<template>
    <div class="app-container">
        <el-form :model="queryParams" ref="queryRef" :inline="true" v-show="showSearch" label-width="68px">
            <el-form-item label="规则名称" prop="name">
                <el-input v-model="queryParams.name" placeholder="规则名称" clearable style="width: 240px" @keyup.enter="handleQuery" />
            </el-form-item>
            <el-form-item>
                <el-button type="primary" icon="Search" @click="handleQuery">搜索</el-button>
                <el-button icon="Refresh" @click="resetQuery">重置</el-button>
            </el-form-item>
        </el-form>
    
        <el-row :gutter="10" class="mb8">
            <el-col :span="1.5">
                <el-button type="primary" plain icon="Plus" @click="handleAdd" v-hasPermi="['DataPermission.Data.Create']">新增</el-button>
            </el-col>
            <right-toolbar v-model:showSearch="showSearch" @queryTable="getList"></right-toolbar>
        </el-row>
    
        <el-table v-loading="loading" :data="typeList" @selection-change="handleSelectionChange">
            <el-table-column type="selection" width="55" align="center" />
            <el-table-column label="规则名称" align="center" prop="name" />
            <el-table-column label="隐藏字段" align="center" prop="hideDataTargetFieldDisplayNameStr" />
            <el-table-column label="操作权限" align="center" prop="dataOperationsStr" />
            <el-table-column label="创建时间" align="center" prop="creationTime" width="180">
            <template #default="scope">
                <span>{{ parseTime(scope.row.creationTime) }}</span>
            </template>
        </el-table-column>
            <el-table-column label="操作" align="center" width="220" class-name="small-padding fixed-width">
                <template #default="scope">
                    <el-button link type="primary" icon="Edit" @click="handleUpdate(scope.row)" v-hasPermi="['DataPermission.Data.Update']">修改</el-button>
                    <el-button link type="primary" icon="Delete" @click="handleDelete(scope.row)" v-hasPermi="['DataPermission.Data.Delete']">删除</el-button>
                </template>
            </el-table-column>
        </el-table>
    
        <pagination v-show="total > 0" :total="total" v-model:page="queryParams.pageIndex" v-model:limit="queryParams.pageSize" @pagination="getList" />
    
        <!-- 添加或修改参数配置对话框 -->
        <!-- <el-dialog :title="title" v-model="open" width="500px" append-to-body> -->
        <el-drawer ref="drawerRef" v-model="open" :title="title" direction="rtl" class="demo-drawer"  size="50%">
    
            <el-form ref="appRef" :model="form" :rules="rules" label-width="80px">
    
                <el-form-item label="数据对象" prop="dataTargetName">
                <el-select v-model="form.dataTargetName" placeholder="数据对象"  :disabled="form.id" clearable @clear="clearValue" style="width: 240px" @change="changeDataTarget">
                    <el-option v-for="target in dataTargets" :key="target.name" :label="target.displayName" :value="target.name" />
                </el-select>
            </el-form-item>

                <el-form-item label="规则名称" prop="name">
                    <el-input v-model="form.name" placeholder="请输入规则名称" />
                </el-form-item>

                <el-form-item label="隐藏字段" prop="hideDataTargetFields">
                <el-select multiple v-model="form.hideDataTargetFields" placeholder="请选择隐藏字段">
                    <el-option v-for="fieldName in dataTargetFields" :key="fieldName.name" :label="fieldName.displayName" :value="fieldName.name"></el-option>
                                </el-select>
               </el-form-item>

               <el-form-item label="操作权限" prop="dataOperations">
                <el-select multiple v-model="form.dataOperations" placeholder="请选择操作权限">
                                    <el-option v-for="compare in RuleDataOperation" :key="compare.value" :label="compare.label" :value="compare.value"></el-option>
                                </el-select>
            </el-form-item>

                <el-form-item label="条件组">
                    <ConditionGroup :form="form" :fieldNames="dataTargetFields" :defineFields="defineFields"></ConditionGroup>
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
        getDataRule,
        createDataRule,
        updateDataRule,
        deleteDataRule,
        listDataRules,
        getTarget,
        listTargets,
        getPredefineFieldValues
    } from "@/api/system/datapermis";
    import {
        deepClone
    } from "@/utils/toolFn"
    import ConditionGroup from './ConditionGroup'
    const {
        proxy
    } = getCurrentInstance();
    
    const {
        RuleType
    } = proxy.useDict("RuleType");
    const {
    RuleDataOperation
} = proxy.useDict("RuleDataOperation");
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
    const dataTargetFields = ref([]);

    const defineFields=ref([]);

    const data = reactive({
        form: {},
        queryParams: {
            pageIndex: 1,
            pageSize: 10,
            dataTargetName: undefined
        },
        rules: {
            name: [{
                required: true,
                message: "规则名称不能为空",
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
        rulesConditioncompare
    } = toRefs(data);
    

/** 获取预定义值 */
function getDefineFields(){
    getPredefineFieldValues().then(response => {
        defineFields.value = response.items;
    });
}


/** 获取数据对象 */
function getDataTargetList() {
    listTargets().then(response => {
        dataTargets.value = response.items;
        getList();
    });
}


function changeDataTarget(targetName){
    form.value.hideDataTargetFields=undefined;
    getTargetFields(targetName)
}

    /** 获取数据对象字段 */
    function getTargetFields(targetName) {
       
        getTarget(targetName).then(response => {
            dataTargetFields.value = response.fields;
        });
    }
    
    
    /** 查询规则列表 */
    function getList() {
        loading.value = true;
        listDataRules(queryParams.value).then(response => {
            typeList.value = response.items;
            total.value = response.totalCount;
            loading.value = false;
            console.log("dataRules",typeList.value)
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
            priority: undefined,
            ruleType: undefined,
            hideDataTargetFields:[],
            dataOperations:[]
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
        const id = row.id || ids.value;
        getDataRule(id).then(response => {
            form.value = response;
            open.value = true;
            title.value = "修改规则";
            getTargetFields(response.dataTargetName)
        });
        // form.value = deepClone(row);
        // open.value = true;
        // title.value = "修改规则";
    }
    /** 提交按钮 */
    function submitForm() {
        proxy.$refs["appRef"].validate(valid => {
            if (valid) {
                if (form.value.id != undefined) {
                    updateDataRule(form.value).then(response => {
                        proxy.$modal.msgSuccess("修改成功");
                        open.value = false;
                        getList();
                    });
                } else {
                    createDataRule(form.value).then(response => {
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
            return deleteDataRule(ids);
        }).then(() => {
            getList();
            proxy.$modal.msgSuccess("删除成功");
        }).catch(() => {});
    }
    
    getDataTargetList();
    getDefineFields();
    </script>
    