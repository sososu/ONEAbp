<template>
    <div class="app-container">
        <el-form :model="queryParams" ref="queryRef" :inline="true" v-show="showSearch" label-width="68px">
            <el-form-item label="版本名称" prop="name">
                <el-input v-model="queryParams.name" placeholder="请输入版本名称" clearable style="width: 240px" @keyup.enter="handleQuery" />
            </el-form-item>
            <el-form-item>
                <el-button type="primary" icon="Search" @click="handleQuery">搜索</el-button>
                <el-button icon="Refresh" @click="resetQuery">重置</el-button>
            </el-form-item>
        </el-form>
    
        <el-row :gutter="10" class="mb8">
            <el-col :span="1.5">
                <el-button type="primary" plain icon="Plus" @click="handleAdd" v-hasPermi="['SysResource.SaleVersion.Create']">新增</el-button>
            </el-col>
            <right-toolbar v-model:showSearch="showSearch" @queryTable="getList"></right-toolbar>
        </el-row>
    
        <el-table v-loading="loading" :data="typeList" @selection-change="handleSelectionChange">
            <el-table-column type="selection" width="55" align="center" />
            <el-table-column label="版本名称" align="center" prop="name" />
            <el-table-column label="价格" align="center" prop="price" />
            <el-table-column label="描述" align="center" prop="description" />
            <el-table-column label="创建时间" align="center" prop="creationTime" width="180">
                <template #default="scope">
                    <span>{{ parseTime(scope.row.creationTime) }}</span>
                </template>
            </el-table-column>
            <el-table-column label="操作" align="center" width="400" class-name="small-padding fixed-width">
                <template #default="scope">
                    <el-button link type="primary" icon="Edit" @click="handleUpdate(scope.row)" v-hasPermi="['SysResource.SaleVersion.Update']">修改</el-button>
                    <el-button link type="primary" icon="Delete" @click="handleDelete(scope.row)" v-hasPermi="['SysResource.SaleVersion.Delete']">删除</el-button>
                    <el-button link type="primary" icon="Menu" @click="handleAuthMenu(scope.row)" v-hasPermi="['SysResource.SaleVersion.Menu']">应用菜单</el-button>
                </template>
            </el-table-column>
        </el-table>
    
        <pagination v-show="total > 0" :total="total" v-model:page="queryParams.pageIndex" v-model:limit="queryParams.pageSize" @pagination="getList" />
    
        <!-- 添加或修改参数配置对话框 -->
        <!-- <el-dialog :title="title" v-model="open" width="500px" append-to-body> -->
                 <el-drawer
        ref="drawerRef"
        v-model="open"
        :title="title"
        direction="rtl"
        class="demo-drawer"
      >
            
            <el-form ref="appRef" :model="form" :rules="rules" label-width="80px">
                <el-form-item label="版本名称" prop="name">
                    <el-input v-model="form.name" placeholder="请输入版本名称" />
                </el-form-item>
                <el-form-item label="价格" prop="price">
                    <el-input v-model="form.price" placeholder="请输入价格" />
                </el-form-item>
                <el-form-item label="描述" prop="description">
                    <el-input v-model="form.description" placeholder="请输入描述" />
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



        <el-drawer
    ref="drawerRef"
    v-model="openMenu"
    size="50%"
    title="菜单权限"
    direction="rtl"
    class="demo-drawer"
  >

        <SaleMenu :saleName="currentSelectedSaleName"  :saleId="currentSelectedSaleId" 
           ref="authMenuRef"
       ></SaleMenu>

         <template #footer>
            <div class="dialog-footer">
               <el-button @click="cancelMenu">取 消</el-button>
            </div>
         </template>
             </el-drawer>


    </div>
    </template>
    
    <script setup name="Sale">
    import SaleMenu from '@/components/SaleMenu'
   import {
    createSaleVersion,
    updateSaleVersion,
    deleteSaleVersion,
    getSaleVersions,
    getSaleVersionMenus,
    SetSaleVersionMenus
} from "@/api/sass/sale";
    import{deepClone}from "@/utils/toolFn"

import useSaleStore from '@/store/modules/sale'
    
const saleStore=useSaleStore();

    const { proxy } = getCurrentInstance();
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
    const openMenu = ref(false);
    const data = reactive({
        form: {},
        queryParams: {
            pageIndex: 1,
            pageSize: 10,
            appName: undefined,
            appCode: undefined
        },
        rules: {
            appName: [{
                required: true,
                message: "销售版本名称不能为空",
                trigger: "blur"
            }],
            appCode: [{
                required: true,
                message: "销售版本编码不能为空",
                trigger: "blur"
            }],
            appVersion:[{
                required: true,
                message: "销售版本版本不能为空",
                trigger: "blur"
            }]
        },
    });
    

    const authMenuRef=ref(null);
    const currentSelectedSaleId = ref('');
    const currentSelectedSaleName=ref('');

    const {
        queryParams,
        form,
        rules
    } = toRefs(data);
    
    /** 查询销售版本列表 */
    function getList() {
        loading.value = true;
        getSaleVersions(queryParams.value).then(response => {
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
    /** 表单重置 */
    function reset() {
        form.value = {
            name: undefined,
            price: undefined,
            description: undefined
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
        title.value = "添加销售版本";
    }
    /** 多选框选中数据 */
    function handleSelectionChange(selection) {
        ids.value = selection.map(item => item.id);
    
        console.log("ids",ids.value);
    
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
        //     title.value = "修改销售版本";
        // });
        form.value=deepClone(row);
        open.value = true;
        title.value = "修改销售版本";
    }
    /** 提交按钮 */
    function submitForm() {
        proxy.$refs["appRef"].validate(valid => {
            if (valid) {
                if (form.value.id != undefined) {
                    saleStore.modifySale(form.value.id,form.value).then(response => {
                        proxy.$modal.msgSuccess("修改成功");
                        open.value = false;
                        getList();
                    });
                } else {
                    saleStore.addSale(form.value).then(response => {
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
        proxy.$modal.confirm('是否确认删除销售版本编号为"' + ids + '"的数据项？').then(function () {
            return saleStore.removeSale(ids);
        }).then(() => {
            getList();
            proxy.$modal.msgSuccess("删除成功");
        }).catch(() => {});
    }
    

/** 取消按钮 */
function cancelMenu() {
  openMenu.value = false;
}

/** 分配菜单权限 */
function handleAuthMenu(row) {
   openMenu.value=true;
   currentSelectedSaleId.value=row.id;
   currentSelectedSaleName.value=row.name;
   nextTick(()=>{
      if(authMenuRef.value){
        console.log('authMenuRef',)
        authMenuRef.value.start();
      }
   })

}

    getList();
    </script>
    