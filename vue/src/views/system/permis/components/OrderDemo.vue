<template>
    <div class="app-container">
        <el-form :model="queryParams" ref="queryRef" :inline="true" v-show="showSearch" label-width="68px">
            <el-form-item label="产品名称" prop="productName">
            <el-input v-model="queryParams.productName" placeholder="请输入产品名称" clearable style="width: 240px" @keyup.enter="handleQuery" />
        </el-form-item>
            <el-form-item>
                <el-button type="primary" icon="Search" @click="handleQuery">搜索</el-button>
                <el-button icon="Refresh" @click="resetQuery">重置</el-button>
            </el-form-item>
        </el-form>
    
        <el-row :gutter="10" class="mb8">
            <el-col :span="1.5">
                <el-button type="primary" plain icon="Plus" @click="handleAdd" v-hasPermi="['SysResource.App.Create']">新增</el-button>
            </el-col>
            <right-toolbar v-model:showSearch="showSearch" @queryTable="getList"></right-toolbar>
        </el-row>
    
        <el-table v-loading="loading" :data="typeList" @selection-change="handleSelectionChange">
            <el-table-column type="selection" width="55" align="center" />
            <el-table-column label="产品名称" align="center" prop="productName" />
            <el-table-column label="金额" align="center" prop="amount" />
            <el-table-column label="收件人" align="center" prop="addressee" />
            <el-table-column label="手机号码" align="center" prop="mobile" />
            <el-table-column label="收件地址" align="center" prop="address" />
           
            <el-table-column label="操作" align="center" width="220" class-name="small-padding fixed-width">
                <template #default="scope">
                    <el-button v-if="scope.row?.extraProperties.CanEdit" link type="primary" icon="Edit" @click="handleUpdate(scope.row)" v-hasPermi="['SysResource.App.Update']">修改</el-button>
                    <el-button v-if="scope.row?.extraProperties.CanRemove" link type="primary" icon="Delete" @click="handleDelete(scope.row)" v-hasPermi="['SysResource.App.Delete']">删除</el-button>
                </template>
            </el-table-column>
        </el-table>
    
        <pagination v-show="total > 0" :total="total" v-model:page="queryParams.pageIndex" v-model:limit="queryParams.pageSize" @pagination="getList" />
    
    </div>
    </template>
    
        
    <script setup>
    import {
        listOrder
    } from "@/api/system/datapermis";
    import {
        deepClone
    } from "@/utils/toolFn"
    
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
    const userRuleKv = ref([]);
    const dataRuleKv = ref([]);
    
    const data = reactive({
        form: {},
        queryParams: {
            pageIndex: 1,
            pageSize: 10,
            productName: undefined
        },
    });
    
    const {
        queryParams,
        form,
    } = toRefs(data);
    
 
    /** 查询规则列表 */
    function getList() {
        loading.value = true;
        listOrder(queryParams.value).then(response => {
            typeList.value = response.items;
            total.value = response.totalCount;
            loading.value = false;
        });
    }
    /** 取消按钮 */
    function cancel() {
        open.value = false;
        reset();
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

    getList();
    </script>
    