<template>
<div class="app-container">
    <el-form :model="queryParams" ref="queryRef" :inline="true" v-show="showSearch" label-width="68px">
        <el-form-item label="租户名称" prop="filter">
            <el-input v-model="queryParams.filter" placeholder="请输入租户名称" clearable style="width: 240px" @keyup.enter="handleQuery" />
        </el-form-item>
        <el-form-item>
            <el-button type="primary" icon="Search" @click="handleQuery">搜索</el-button>
            <el-button icon="Refresh" @click="resetQuery">重置</el-button>
        </el-form-item>
    </el-form>

    <el-row :gutter="10" class="mb8">
        <el-col :span="1.5">
            <el-button type="primary" plain icon="Plus" @click="handleAdd" v-hasPermi="['AbpTenantManagement.Tenants.Create']">新增</el-button>
        </el-col>
        <right-toolbar v-model:showSearch="showSearch" @queryTable="getList"></right-toolbar>
    </el-row>

    <el-table v-loading="loading" :data="typeList" @selection-change="handleSelectionChange">
        <el-table-column type="selection" width="55" align="center" />
        <el-table-column label="租户名称" align="center" prop="name" />
        <el-table-column label="联系人" align="center" prop="contact" />
        <el-table-column label="联系方式" align="center" prop="contactWay" />
        <el-table-column label="到期时间" align="center" prop="expirationDate" />
        <el-table-column label="创建时间" align="center" prop="creationTime" width="180">
            <template #default="scope">
                <span>{{ parseTime(scope.row.creationTime) }}</span>
            </template>
        </el-table-column>
        <el-table-column label="操作" align="center" width="400" class-name="small-padding fixed-width">
            <template #default="scope">
                <el-button link type="primary" icon="Edit" @click="handleUpdate(scope.row)" v-hasPermi="['AbpTenantManagement.Tenants.Update']">修改</el-button>
                <el-button link type="primary" icon="Refresh" @click="handleRefreshSaleVersionMenus(scope.row)" v-hasPermi="['AbpTenantManagement.Tenants.Update']">刷新菜单</el-button>
                <el-button link type="primary" icon="Delete" @click="handleDelete(scope.row)" v-hasPermi="['AbpTenantManagement.Tenants.Delete']">删除</el-button>
                <el-button link type="primary" @click="handleLogin(scope.row)" v-hasPermi="['AbpTenantManagement.Tenants.Delete']">登录租户</el-button>
            </template>
        </el-table-column>
       </el-table>

    <pagination v-show="total > 0" :total="total" v-model:page="queryParams.pageIndex" v-model:limit="queryParams.pageSize" @pagination="getList" />

    <!-- 添加租户配置对话框 -->
    <el-dialog :title="title" v-model="open" width="800px" append-to-body>
        <div>
            <el-steps :active="active" :finish-status="stepFinishStatus" align-center>
                <el-step title="填写租户信息" />
                <el-step title="填写数据库连接信息" />
                <el-step title="初始化" />
            </el-steps>
            <section class="dialog_section">
                <el-form ref="appRef" :model="form" :rules="rules" label-width="80px">
                    <div v-if="active == 0">
                        <el-form-item label="租户名称" prop="name" :rules="{
        required: true,
        message: '名称不能为空',
        trigger: 'blur',
      }">
                            <el-input v-model="form.name" placeholder="请输入租户名称" />
                        </el-form-item>
                        <el-form-item label="联系人" prop="contact">
                            <el-input v-model="form.contact" placeholder="请输入联系人" />
                        </el-form-item>
                        <el-form-item label="联系方式" prop="contactWay">
                            <el-input v-model="form.contactWay" placeholder="请输入联系方式" />
                        </el-form-item>

                        <el-form-item label="租户邮箱" prop="initData.adminEmailAddress" :rules="{
        required: true,
        message: '租户邮箱不能为空',
        trigger: 'blur',
      }">
                            <el-input v-model="form.initData.adminEmailAddress" placeholder="请输入租户邮箱" />
                        </el-form-item>

                        <el-form-item label="租户密码" prop="initData.adminPassword" :rules="{
        required: true,
        message: '租户密码不能为空',
        trigger: 'blur',
      }">
                            <el-input v-model="form.initData.adminPassword"  type="password" placeholder="请输入租户密码" show-password/>
                        </el-form-item>

                        <el-form-item label="销售版本" prop="saleVersionId" :rules="{
        required: true,
        message: '请选择销售版本',
        trigger: 'blur',
      }">
                            <el-select v-model="form.saleVersionId" placeholder="请选择">
                                <el-option v-for="item in saleStore.saleVersions" :key="item.id" :label="item.name" :value="item.id"></el-option>
                            </el-select>
                        </el-form-item>

                        <el-form-item label="到期时间" prop="expirationDate" :rules="{
        required: true,
        message: '请选择到期时间',
        trigger: 'blur',
      }">
                            <el-date-picker v-model="form.expirationDate" type="date" :disabled-date="disabledDate" placeholder="请选择到期时间" clearable></el-date-picker>
                        </el-form-item>

                    </div>

                    <div v-if="active == 1">

                        <el-checkbox v-model="sharedConnetced" label="共享连接" size="large" border style="margin-bottom: 20px;" />

                        <div v-if="!sharedConnetced" class="connectionstr_setting_div">
                            <h3>连接字符串设置</h3>
                            <div v-for="(domain,index) in form.connectionStrings" :key="index">

                                <el-row>
                                    <el-col :span="6">
                                        <el-form-item :label="'名称'" :prop="'connectionStrings.' + index + '.name'" :rules="{
        required: true,
        message: '名称不能为空',
        trigger: 'blur',
      }">
                                            <el-input v-model="domain.name" />

                                        </el-form-item>

                                    </el-col>
                                    <el-col :span="14">
                                        <el-form-item :label="'值'" :prop="'connectionStrings.' + index + '.value'" :rules="{
        required: true,
        message: '值不能为空',
        trigger: 'blur',
      }">
                                            <el-input v-model="domain.value" />

                                        </el-form-item>
                                    </el-col>
                                    <el-col :span="4">
                                        <el-button class="mt-2" @click.prevent="removeDomain(domain)">移除</el-button>
                                    </el-col>
                                </el-row>

                            </div>

                            <el-button @click="addDomain">添加</el-button>
                        </div>

                    </div>

                    <div v-if="active==2" style="width: 100%; align-content: center;">
                        <el-progress style="margin-bottom: 20px;"
     :stroke-width="26"              
      :percentage="percentage"
      :indeterminate="true"
      :status="progressStatus"
      :duration="5"
    />
                    </div>

                </el-form>
                <div class="dailog_footer">
                    <el-button v-if="active > 0 && showPrev" type="info" icon="el-icon-arrow-left" @click="prev">上一步</el-button>
                    <el-button v-if="active < step - 2" type="primary" icon="el-icon-arrow-right" @click="next">下一步</el-button>
                    <el-button v-if="active == step - 2" type="primary" @click="init">初始化</el-button>
                </div>
            </section>
        </div>
    </el-dialog>

     <!-- 修改租户基本信息对话框 -->
    <el-drawer ref="drawerRef" v-model="openEdit" :title="title" direction="rtl" class="demo-drawer">
        <el-form ref="appBaseRef" :model="formBase" :rules="rules" label-width="80px">
            <el-form-item label="租户名称" prop="name">
                            <el-input v-model="formBase.name" placeholder="请输入租户名称" />
                        </el-form-item>
                        <el-form-item label="联系人" prop="contact">
                            <el-input v-model="formBase.contact" placeholder="请输入联系人" />
                        </el-form-item>
                        <el-form-item label="联系方式" prop="contactWay">
                            <el-input v-model="formBase.contactWay" placeholder="请输入联系方式" />
                        </el-form-item>

                        <el-form-item label="到期时间" prop="expirationDate" >
                            <el-date-picker v-model="formBase.expirationDate" type="date" :disabled-date="disabledDate" placeholder="请选择到期时间" clearable></el-date-picker>
                        </el-form-item>

                        <el-form-item label="销售版本" prop="saleVersionId">
                            <el-select v-model="formBase.saleVersionId" placeholder="请选择">
                                <el-option v-for="item in saleStore.saleVersions" :key="item.id" :label="item.name" :value="item.id"></el-option>
                            </el-select>
                        </el-form-item>
        </el-form>
        <template #footer>
            <div class="dialog-footer">
                <el-button type="primary" @click="submitForm">确 定</el-button>
                <el-button @click="cancel">取 消</el-button>
            </div>
        </template>
    </el-drawer>

</div>
</template>

<script setup name="Tenant">
import {
    createTenant,
    updateTenant,
    deleteTenant,
    getTenants,
    updateTenantSaleVersion
} from "@/api/sass/tenant";
import {
    getSaleVersions
} from "@/api/sass/sale";

import {
    deepClone
} from "@/utils/toolFn"
import {
    nextTick,
    ref
} from "vue";
import useUserStore from '@/store/modules/user'

import useSaleStore from '@/store/modules/sale'
import useTenantStore from '@/store/modules/tenant'

const tenantStore = useTenantStore();
const saleStore=useSaleStore();

const {
    proxy
} = getCurrentInstance();
const userStore = useUserStore()
const typeList = ref([]);
const openEdit = ref(false);
const open = ref(false);
const loading = ref(true);
const showSearch = ref(true);
const ids = ref([]);
const single = ref(true);
const multiple = ref(true);
const total = ref(0);
const title = ref("");
const dateRange = ref([]);
const saleVersionOptions = ref([]);
const active = ref(0);
const step = ref(3);
const percentage=ref(1);
const showPrev=ref(true);
const stepFinishStatus=ref('success');
const progressStatus=ref('success');
const data = reactive({
    form: {},
    formBase:{},
    queryParams: {
        pageIndex: 1,
        pageSize: 10,
        filter: undefined
    },
    rules: {
        name: [{
            required: true,
            message: "租户名称不能为空",
            trigger: "blur"
        }
    ],
    expirationDate: [{
                required: true,
                message: "请选择到期时间",
                trigger: "blur"
            }],
            saleVersionId: [{
                required: true,
                message: "请选择销售版本",
                trigger: "blur"
            }],
    },
});

const {
    queryParams,
    form,
    formBase,
    rules
} = toRefs(data);

const sharedConnetced = ref(true);
const defaultConnectionStringName = "Default";

function removeDomain(item) {
    const index = form.value.connectionStrings.indexOf(item)
    if (index !== -1) {
        form.value.connectionStrings.splice(index, 1)
    }
}

function addDomain() {
    if (!form.value.connectionStrings) {
        form.value.connectionStrings = [];
    }

    if (form.value.connectionStrings.length < 1) {
        console.log('连接小于1', defaultConnectionStringName)
        form.value.connectionStrings.push({
            name: defaultConnectionStringName,
            value: '',
        })
    } else {
        form.value.connectionStrings.push({
            name: '',
            value: '',
        })
    }

}

/** 查询销售版本列表 */
function getSaleVersionOptions() {
    saleStore.listSale().then(response => {
        saleVersionOptions.value = response.items;
    });
}

/** 查询应用列表 */
function getList() {
    loading.value = true;
    getTenants(queryParams.value).then(response => {
        typeList.value = response.items;
        total.value = response.totalCount;
        loading.value = false;
    });
}
/** 取消按钮 */
function cancel() {
    openEdit.value = false;
    reset();
}
/** 表单重置 */
function reset() {
    form.value = {
        name: undefined,
        contact: undefined,
        contactWay:undefined,
        expirationDate: undefined,
        isActive: true,
        connectionStrings: [],
        initData:{},
        saleVersionId:undefined
    };
    formBase.value={
        name: undefined,
        contact: undefined,
        contactWay:undefined,
        expirationDate: undefined,
        isActive: true,
        saleVersionId:undefined
    };
    proxy.resetForm("appRef");
    proxy.resetForm("appBaseRef");
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
	active.value = 0;
    open.value = true;
    title.value = "添加租户";
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
    formBase.value = deepClone(row);
    openEdit.value = true;
    title.value = "修改租户";
}


/**强制刷新菜单 */
function handleRefreshSaleVersionMenus(row){
    updateTenantSaleVersion(row.id,row.saleVersionId).then(res=>{
        proxy.$modal.msgSuccess("刷新成功");
    })
}


/** 提交按钮 */
function submitForm() {
    proxy.$refs["appBaseRef"].validate(valid => {
  
        if (valid) {
            if (formBase.value.id != undefined) {
                tenantStore.modifyTenant(formBase.value).then(response => {
                    proxy.$modal.msgSuccess("修改成功");
                    openEdit.value = false;
                    getList();
                });
            } 
        }
    });
}
/** 删除按钮操作 */
function handleDelete(row) {
    const ids = row.id || ids.value;
    proxy.$modal.confirm('是否确认删除应用编号为"' + ids + '"的数据项？').then(function () {
        return tenantStore.removeTenant(ids);
    }).then(() => {
        getList();
        proxy.$modal.msgSuccess("删除成功");
    }).catch(() => {});
}



/** 登录按钮操作 */
function handleLogin(row) {
    const id = row.id;
    console.log("login tenant")
    userStore.exchangeLogin(id).then(()=>{
    location.href = '/index';
  }
  ).catch(() => { });
}

async function next() {
    if (active.value === 0) {
        var a1 = await proxy.$refs["appRef"].validateField('name', (valid) => {
            return valid;
        })
        var a2 = await proxy.$refs["appRef"].validateField('saleVersionId', (valid) => {
            return valid;
        })
        var a3 = await proxy.$refs["appRef"].validateField('expirationDate', (valid) => {
            return valid;
        })
        var a4 = await proxy.$refs["appRef"].validateField('initData.adminEmailAddress', (valid) => {
            return valid;
        })
        var a5 = await proxy.$refs["appRef"].validateField('initData.adminPassword', (valid) => {
            return valid;
        })
        if (a1 && a2 && a3 && a4 && a5) {
            active.value++
        }

    } else{
        if (++active.value > step.value-1) active.value = 0
    }
}

function prev() {
    if (--active.value < 0) active.value = 0
}

function init() {
          if(sharedConnetced.value){
            form.value.connectionStrings=[] 
          }

          proxy.$refs["appRef"].validate(valid => {
                if (valid) {
                    active.value++
                    showPrev.value=false;
                            
                    tenantStore.addTenant(form.value).then(response=>{
                        active.value++
                        percentage.value=100;
                        progressStatus.value='success';
                        stepFinishStatus.value='success';
                        open.value = false;
                        getList();
                    }).catch(error=>{
                        percentage.value=0;
                        showPrev.value=true;
                        progressStatus.value='exception';
                        stepFinishStatus.value='error';
                    })
                }});

    //   if (!sharedConnetced.value) {
    //         let isValid = true;
    //         if (form.connectionStrings?.length > 0) {
    //             for (var item of form.connectionStrings) {
    //                 isValid &&= item.name && item.value
    //             }
    //             if (isValid) {
            
    //          }
    //         }
    //     } else {
    //         active.value++
    //     }

    //     active.value++
}

const disabledDate = (time) => {
    return time.getTime() < Date.now()
}

getList();
getSaleVersionOptions();
</script>

<style lang="scss" scoped>
.dialog_section {
    margin: 0 auto 30px;
    padding-top: 30px;
    // min-height: 300px;
    // padding-right: 100px;
    align-content: center;

    .status {
        font-size: 12px;
        margin-left: 20px;
        color: #e6a23c;
    }
}

.connectionstr_setting_div {
    margin: 0 auto 30px;
}
</style>
