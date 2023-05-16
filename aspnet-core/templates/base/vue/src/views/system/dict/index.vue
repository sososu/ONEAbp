<template>
<div class="app-container">
    <el-row :gutter="20">
        <el-col :span="4" :xs="24">
            <div class="head-container">
                <el-input v-model="categoreyName" placeholder="请输入字典类别" clearable prefix-icon="Search" style="margin-bottom: 20px"  @change="matchcategorey($event)"/>
            </div>
            <div class="head-container">
                <el-table :data="typeList" style="width: 100%" @row-click="getItems">
                    <el-table-column label="类别" prop="name" />
                    <el-table-column align="right">
                        <template #header>
                            <el-button type="primary" plain icon="Plus" size="small" @click="handleAdd(true)" v-hasPermi="['DataDictionary.Base.Create']"></el-button>
                        </template>
                        <template #default="scope">

                            <el-dropdown>
                                <el-button text> <el-icon :size="20" color="#4682B4"><MoreFilled /></el-icon></el-button>
                                <template #dropdown>
                                    <el-dropdown-menu>
                                        <el-dropdown-item>
                                            <el-button size="small" icon="Edit" @click="handleUpdate(scope.row,true)"></el-button>
                                        </el-dropdown-item>
                                        <el-dropdown-item>
                                            <el-button size="small" type="danger" icon="Delete" @click="handleDelete(scope.row)"></el-button>
                                        </el-dropdown-item>
                                    </el-dropdown-menu>
                                </template>
                            </el-dropdown>
                        </template>
                    </el-table-column>
                </el-table>
            </div>

        </el-col>
        <el-col :span="20" :xs="24">
            <el-row :gutter="10" class="mb8">
                <el-col :span="1.5">
                    <el-button type="primary" plain icon="Plus" @click="handleAdd" v-hasPermi="['DataDictionary.Base.Create']">新增</el-button>
                </el-col>
                <!-- <el-col :span="1.5">
                    <el-button type="success" plain icon="Edit" :disabled="single" @click="handleUpdate" v-hasPermi="['DataDictionary.Base.Update']">修改</el-button>
                </el-col>
                <el-col :span="1.5">
                    <el-button type="danger" plain icon="Delete" :disabled="multiple" @click="handleDelete" v-hasPermi="['DataDictionary.Base.Delete']">删除</el-button>
                </el-col> -->
                <!-- <el-col :span="1.5">
                    <el-button type="warning" plain icon="Download" @click="handleExport" v-hasPermi="['system:dict:export']">导出</el-button>
                </el-col>
                <el-col :span="1.5">
                    <el-button type="danger" plain icon="Refresh" @click="handleRefreshCache" v-hasPermi="['system:dict:remove']">刷新缓存</el-button>
                </el-col> -->
                <right-toolbar v-model:showSearch="showSearch" @queryTable="getCategorys"></right-toolbar>
            </el-row>

            <el-table v-loading="loading" :data="childTypeList" @selection-change="handleSelectionChange">
                <el-table-column type="selection" width="55" align="center" />
                <!-- <el-table-column label="字典编号" align="center" prop="id" /> -->
                <el-table-column label="字典名称" align="center" prop="name" :show-overflow-tooltip="true" />
                <el-table-column label="字典编码" align="center" prop="code"  :show-overflow-tooltip="true" />
                <el-table-column label="字典值" align="center" prop="value"  :show-overflow-tooltip="true" />
                <el-table-column label="状态" align="center" prop="statusStr"  :show-overflow-tooltip="true" />
                <el-table-column label="来源" align="center" prop="sourceStr"  :show-overflow-tooltip="true" />
                <el-table-column label="备注" align="center" prop="description" :show-overflow-tooltip="true" />
                <el-table-column label="操作" align="center" width="160" class-name="small-padding fixed-width">
                    <template #default="scope">
                        <el-button link type="primary" icon="Edit" @click="handleUpdate(scope.row)" v-hasPermi="['DataDictionary.Base.Update']">修改</el-button>
                        <el-button link type="primary" icon="Delete" @click="handleDelete(scope.row)" v-hasPermi="['DataDictionary.Base.Delete']">删除</el-button>
                    </template>
                </el-table-column>
            </el-table>

            <pagination v-show="total > 0" :total="total" v-model:page="queryParams.pageNum" v-model:limit="queryParams.pageSize" @pagination="getCategorys" />
        </el-col>
    </el-row>
    <!-- 添加或修改参数配置对话框 -->
    <el-drawer
    ref="drawerRef"
    v-model="open"
    :title="title"
    :direction="direction"
    size="350"
    class="demo-drawer"
  >
    <!-- <el-dialog :title="title" v-model="open" width="500px" append-to-body> -->
        <el-form ref="dictRef" :model="form" :rules="rules" label-width="80px">
            <el-form-item label="字典名称" prop="name">
                <el-input v-model="form.name" placeholder="请输入字典名称" />
            </el-form-item>
            <el-form-item label="字典编码" prop="code">
                <el-input v-model="form.code" placeholder="请输入字典类型"  disabled="form.source==0"/>
            </el-form-item>
            <el-form-item label="字典值" prop="value" v-if="!isCategoryInput">
                <el-input v-model="form.value" placeholder="请输入字典值"  disabled="form.source==0"/>
            </el-form-item>
            <el-form-item label="状态" prop="status">
                <el-radio-group v-model="form.status">
                    <el-radio v-for="dict in DataItemStatus" :key="dict.value" :label="dict.value">{{ dict.label }}</el-radio>
                </el-radio-group>
            </el-form-item>
            <el-form-item label="排序" prop="order">
               <el-input-number v-model="form.order" controls-position="right" :min="0" />
            </el-form-item>
            <el-form-item label="备注" prop="description">
                <el-input v-model="form.description" type="textarea" placeholder="请输入内容"></el-input>
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

<script setup name="Dict">
import useDictStore from '@/store/modules/dict'
import {
    getDataDictionaryItems,
    updateDataDictionaryItem,
    createDataDictionaryItem,
    getDataDictionaryCategorys,
    updateDataDictionaryCategory,
    createDataDictionaryCategory,
    deleteDictionary
} from "@/api/system/dictionaries";
import{deepClone}from "@/utils/toolFn"
const {
    proxy
} = getCurrentInstance();
const {
    DataItemStatus
} = proxy.useDict("DataItemStatus");

const categoreyName=ref('')
const direction=ref("rtl")
const typeList = ref([]);
const fixedtypeList = ref([]);
const childTypeList = ref([]);
const open = ref(false);
const loading = ref(true);
const showSearch = ref(true);
const ids = ref([]);
const single = ref(true);
const multiple = ref(true);
const total = ref(0);
const title = ref("");
const dateRange = ref([]);

const isCategoryInput=ref(false);

const data = reactive({
    form: {},
    queryParams: {
        pageNum: 1,
        pageSize: 10,
        dictName: undefined,
        dictType: undefined,
        status: undefined
    },
    rules: {
        dictName: [{
            required: true,
            message: "字典名称不能为空",
            trigger: "blur"
        }],
        dictType: [{
            required: true,
            message: "字典类型不能为空",
            trigger: "blur"
        }]
    },
});

const {
    queryParams,
    form,
    rules
} = toRefs(data);

/** 前缀匹配字典类型列表 */
function matchcategorey(prefix){
    let result = fixedtypeList.value.filter(item=> item.name.startsWith(prefix))
    typeList.value = result;
};  

/** 查询字典类型列表 */
function getCategorys() {
    loading.value = true;
    getDataDictionaryCategorys().then(response => {
        typeList.value = response.items;
        fixedtypeList.value=response.items;
        loading.value = false;
        if(typeList.value.length>0){
         getItems(typeList.value[0])
        }
    });
}
/** 查询字典项列表 */
function getItems(row) {
    loading.value = true;
    getDataDictionaryItems({categoryId:row.id}).then(response => {
      childTypeList.value = response.items;
        loading.value = false;
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
        id: undefined,
        name: undefined,
        code: undefined,
        status: "0",
        description: undefined,
        order:0
    };
    proxy.resetForm("dictRef");
}
/** 搜索按钮操作 */
function handleQuery() {
    queryParams.value.pageNum = 1;
    getCategorys();
}
/** 重置按钮操作 */
function resetQuery() {
    dateRange.value = [];
    proxy.resetForm("queryRef");
    handleQuery();
}
/** 新增按钮操作 */
function handleAdd(isCategory) {
    reset();
    open.value = true;
    title.value = "添加字典类型";
    isCategoryInput.value=isCategory
}
/** 多选框选中数据 */
function handleSelectionChange(selection) {
    ids.value = selection.map(item => item.dictId);
    single.value = selection.length != 1;
    multiple.value = !selection.length;
}
/** 修改按钮操作 */
function handleUpdate(row,isCategory) {
    reset();
    open.value = true;
    form.value=deepClone(row);
    title.value = "修改字典类型";
    isCategoryInput.value=isCategory
}
/** 提交按钮 */
function submitForm() {
    proxy.$refs["dictRef"].validate(valid => {
        if (valid) {
            if (form.value.id != undefined) {
                updateDataDictionaryCategory(form.value.id,form.value).then(response => {
                    proxy.$modal.msgSuccess("修改成功");
                    open.value = false;
                    getCategorys();
                });
            } else {
                createDataDictionaryCategory(form.value).then(response => {
                    proxy.$modal.msgSuccess("新增成功");
                    open.value = false;
                    getCategorys();
                });
            }
        }
    });
}
/** 删除按钮操作 */
function handleDelete(row) {
    const dictIds = row.id || ids.value;
    proxy.$modal.confirm('是否确认删除字典编号为"' + dictIds + '"的数据项？').then(function () {
        return deleteDictionary(dictIds);
    }).then(() => {
        getCategorys();
        proxy.$modal.msgSuccess("删除成功");
    }).catch(() => {});
}

getCategorys()

</script>

<style scoped lang="scss">

</style>
