<template>
<div class="app-container">
    <el-form :model="queryParams" ref="queryRef" :inline="true" v-show="showSearch" label-width="68px">
        <el-form-item label="数据对象" prop="dataTargetName">
            <el-select v-model="queryParams.dataTargetName" placeholder="数据对象" clearable @clear="clearValue" style="width: 240px">
                <el-option v-for="target in dataTargets" :key="target.name" :label="target.name" :value="target.name" />
            </el-select>
        </el-form-item>
        <el-form-item>
            <el-button type="primary" icon="Search" @click="handleQuery">搜索</el-button>
            <el-button icon="Refresh" @click="resetQuery">重置</el-button>
        </el-form-item>
    </el-form>

    <el-row :gutter="10" class="mb8">
        <el-col :span="1.5">
            <el-button type="primary" plain icon="Plus" @click="handleAdd" v-hasPermi="['DataPermission.Rule.Create']">新增</el-button>
        </el-col>
        <right-toolbar v-model:showSearch="showSearch" @queryTable="getList"></right-toolbar>
    </el-row>

    <el-table v-loading="loading" :data="typeList" @selection-change="handleSelectionChange">
        <el-table-column type="selection" width="55" align="center" />
        <el-table-column label="数据对象" align="center" prop="targetDisplayName" />
        <el-table-column label="用户规则" align="center" prop="userRuleName" />
        <el-table-column label="数据规则" align="center" prop="dataRuleName" />
        <el-table-column label="规则类型" align="center" prop="ruleTypeStr" />
        <el-table-column label="状态" align="center" prop="isEnable" width="180">
            <template #default="scope">
                <el-switch v-model="scope.row.isEnable"  @change="setIsEnable(scope.row,$event)"/>
            </template>
        </el-table-column>
        
        <el-table-column label="优先级" align="center" prop="priority" />
        <el-table-column label="创建时间" align="center" prop="creationTime" width="180">
            <template #default="scope">
                <span>{{ parseTime(scope.row.creationTime) }}</span>
            </template>
        </el-table-column>
        
        <el-table-column label="操作" align="center" width="220" class-name="small-padding fixed-width">
            <template #default="scope">
                <el-button link type="primary" icon="Edit" @click="handleUpdate(scope.row)" v-hasPermi="['DataPermission.Rule.Update']">修改</el-button>
                <el-button link type="primary" icon="Delete" @click="handleDelete(scope.row)" v-hasPermi="['DataPermission.Rule.Delete']">删除</el-button>
            </template>
        </el-table-column>
    </el-table>

    <pagination v-show="total > 0" :total="total" v-model:page="queryParams.pageIndex" v-model:limit="queryParams.pageSize" @pagination="getList" />

    <!-- 添加或修改参数配置对话框 -->
    <!-- <el-dialog :title="title" v-model="open" width="500px" append-to-body> -->
    <el-drawer ref="drawerRef" v-model="open" :title="title" direction="rtl" class="demo-drawer">

        <el-form ref="appRef" :model="form" :rules="rules" label-width="80px">
            <el-form-item label="数据对象" prop="dataTargetName">
                <el-select v-model="form.dataTargetName" placeholder="数据对象" :disabled="form.id" clearable @clear="clearValue" style="width: 240px" @change="changeDataTarget">
                    <el-option v-for="target in dataTargets" :key="target.name" :label="target.displayName" :value="target.name" />
                </el-select>
            </el-form-item>
            <el-form-item label="用户规则" prop="userRuleId">
                <el-select v-model="form.userRuleId" placeholder="用户规则" clearable @clear="clearValue" style="width: 240px">
                    <el-option v-for="kv in userRuleKv" :key="kv.id" :label="kv.name" :value="kv.id" />
                </el-select>
            </el-form-item>

            <el-form-item label="数据规则" prop="dataRuleId">
                <el-select v-model="form.dataRuleId" placeholder="数据规则" clearable @clear="clearValue" style="width: 240px">
                    <el-option v-for="kv in dataRuleKv" :key="kv.id" :label="kv.name" :value="kv.id" />
                </el-select>
            </el-form-item>

            <el-form-item label="规则类型" prop="ruleType">
                <el-select v-model="form.ruleType" placeholder="规则类型" clearable @clear="clearValue" style="width: 240px">
                    <el-option v-for="dict in RuleType" :key="dict.value" :label="dict.label" :value="dict.value" />
                </el-select>
            </el-form-item>

            <el-form-item prop="priority">
                <template #label>
                            <span>
                                <el-tooltip content="数值越大优先级越高" placement="top">
                                    <el-icon>
                                        <question-filled />
                                    </el-icon>
                                </el-tooltip>
                                优先级
                            </span>
                        </template>
                <el-input-number v-model="form.priority" class="mx-4" :min="1" :max="100" controls-position="right" @change="handleChange" />
            </el-form-item>
        </el-form>
        <template #footer>
            <div class="dialog-footer">
                <el-button type="primary" @click="submitForm">确 定</el-button>
                <el-button @click="cancel">取 消</el-button>
            </div>
        </template>
    </el-drawer>
    <!-- </el-dialog> -->
</div>
</template>

    
<script setup>
import {
    createRule,
    updateRule,
    deleteRule,
    listRules,
    listTargets,
    getUserRuleKv,
    getDataRuleKv,
    setEnable
} from "@/api/system/datapermis";
import {
    deepClone
} from "@/utils/toolFn"
import useRuleStore from '@/store/modules/rule'

const ruleStore = useRuleStore()
const {
    proxy
} = getCurrentInstance();

const {
    RuleType
} = proxy.useDict("RuleType");

const typeList = ref([]);
const open = ref(false);
const loading = ref(true);
const showSearch = ref(true);
const ids = ref([]);
const single = ref(true);
const multiple = ref(true);
const total = ref(0);
const title = ref("");
const dateRange = ref([]);

const dataTargets = ref([]);
const userRuleKv = computed(() => ruleStore.userRules);
//const userRuleKv = storeToRefs(ruleStore.userRules);
const dataRuleKv = ref([]);

const data = reactive({
    form: {},
    queryParams: {
        pageIndex: 1,
        pageSize: 10,
        dataTargetName: undefined
    },
    rules: {
        dataTargetName: [{
            required: true,
            message: "数据对象不能为空",
            trigger: "blur"
        }],
        userRuleId: [{
            required: true,
            message: "用户规则不能为空",
            trigger: "blur"
        }],
        dataRuleId: [{
            required: true,
            message: "用户规则不能为空",
            trigger: "blur"
        }],

        ruleType: [{
            required: true,
            message: "数据类型不能为空",
            trigger: "blur"
        }]
    },
});

const {
    queryParams,
    form,
    rules
} = toRefs(data);

/** 获取数据对象 */
function getDataTargetList() {
    listTargets().then(response => {
        dataTargets.value = response.items;
        getList();
    });
}

// /** 获取用户规则kv */
// function getUserRuleIdName() {
//     getUserRuleKv().then(response => {
//         userRuleKv.value = response.items;
//     });
// }

function changeDataTarget(targetName){
    form.value.dataRuleId=undefined;
    getDataRuleIdName(targetName);
   
}

/** 获取数据规则kv */
function getDataRuleIdName(targetName) {
    getDataRuleKv({
        targetName: targetName
    }).then(response => {
        dataRuleKv.value = response.items;
    });
}

/** 查询规则列表 */
function getList() {
    loading.value = true;
    listRules(queryParams.value).then(response => {
        typeList.value = response.items;
        total.value = response.totalCount;
        loading.value = false;

        typeList.value.forEach(obj => {
  obj.targetDisplayName =dataTargets.value.find(d=>d.name==obj.dataTargetName)?.displayName;
});
        console.log("rules",typeList.value);
    });
}
/** 取消按钮 */
function cancel() {
    open.value = false;
    reset();
}
/** 表单重置 */
function reset() {
    form.value = {
        dataTargetName: undefined,
        userRuleId: undefined,
        dataRuleId: undefined,
        priority: 1,
        ruleType: 0
    };
    proxy.resetForm("appRef");
}
/** 搜索按钮操作 */
function handleQuery() {
    queryParams.value.pageIndex = 1;
    getList();
}
/** 重置按钮操作 */
function resetQuery() {
    dateRange.value = [];
    proxy.resetForm("queryRef");
    handleQuery();
}
/** 新增按钮操作 */
function handleAdd() {
    reset();
    dataRuleKv.value={};
    open.value = true;
    title.value = "添加规则";
}
/** 多选框选中数据 */
function handleSelectionChange(selection) {
    ids.value = selection.map(item => item.id);

    console.log("ids", ids.value);

    single.value = selection.length != 1;
    multiple.value = !selection.length;
}
/** 修改按钮操作 */
function handleUpdate(row) {
    reset();
    // const id = row.id || ids.value;
    // getType(id).then(response => {
    //     form.value = response.data;
    //     open.value = true;
    //     title.value = "修改规则";
    // });
    form.value = deepClone(row);
    getDataRuleIdName(row.dataTargetName);
    open.value = true;
    title.value = "修改规则";
}
/** 提交按钮 */
function submitForm() {
    proxy.$refs["appRef"].validate(valid => {
        if (valid) {
            if (form.value.id != undefined) {
                updateRule(form.value).then(response => {
                    proxy.$modal.msgSuccess("修改成功");
                    open.value = false;
                    getList();
                });
            } else {
                createRule(form.value).then(response => {
                    proxy.$modal.msgSuccess("新增成功");
                    open.value = false;
                    getList();
                });
            }
        }
    });
}
/** 删除按钮操作 */
function handleDelete(row) {
    const ids = row.id || ids.value;
    proxy.$modal.confirm('是否确认删除规则编号为"' + ids + '"的数据项？').then(function () {
        return deleteRule(ids);
    }).then(() => {
        getList();
        proxy.$modal.msgSuccess("删除成功");
    }).catch(() => {});
}


function setIsEnable(row,val){
    var msg=val?"启用":"禁用";
    proxy.$modal.confirm('是否确认' + msg + row.id+'数据项？').then(function () {
        return setEnable(row.id,{isEnable:val}); 
    }).then(() => {
        getList();
        proxy.$modal.msgSuccess(msg+"成功");
    }).catch(() => {});
}
getDataTargetList();

ruleStore.getUserRules();
</script>
