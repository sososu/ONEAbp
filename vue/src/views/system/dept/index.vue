<template>
   <div class="app-container">
      <el-form :model="queryParams" ref="queryRef" :inline="true" v-show="showSearch">
         <el-form-item label="部门名称" prop="name">
            <el-input
               v-model="queryParams.name"
               placeholder="请输入部门名称"
               clearable
               style="width: 200px"
               @keyup.enter="handleQuery"
            />
         </el-form-item>
         <el-form-item>
            <el-button type="primary" icon="Search" @click="handleQuery">搜索</el-button>
            <el-button icon="Refresh" @click="resetQuery">重置</el-button>
         </el-form-item>
      </el-form>

      <el-row :gutter="10" class="mb8">
         <el-col :span="1.5">
            <el-button
               type="primary"
               plain
               icon="Plus"
               @click="handleAdd"
               v-hasPermi="['AbpIdentity.OrganizationUnit.Create']"
            >新增</el-button>
         </el-col>
         <el-col :span="1.5">
            <el-button
               type="info"
               plain
               icon="Sort"
               @click="toggleExpandAll"
            >展开/折叠</el-button>
         </el-col>
         <right-toolbar v-model:showSearch="showSearch" @queryTable="getList"></right-toolbar>
      </el-row>

      <el-table
         v-if="refreshTable"
         v-loading="loading"
         :data="deptList"
         row-key="id"
         :default-expand-all="isExpandAll"
         :tree-props="{ children: 'children', hasChildren: 'hasChildren' }"
      >
         <el-table-column prop="displayName" label="部门名称" width="260"></el-table-column>
         <el-table-column prop="code" label="部门编码" width="200"></el-table-column>
         <!-- <el-table-column label="创建时间" align="center" prop="createTime" width="200">
            <template #default="scope">
               <span>{{ parseTime(scope.row.createTime) }}</span>
            </template>
         </el-table-column> -->
         <el-table-column label="操作" align="center" class-name="small-padding fixed-width">
            <template #default="scope">
               <el-button link type="primary" icon="Edit" @click="handleUpdate(scope.row)" v-hasPermi="['AbpIdentity.OrganizationUnit.Update']">修改</el-button>
               <el-button link type="primary" icon="Plus" @click="handleAdd(scope.row)" v-hasPermi="['AbpIdentity.OrganizationUnit.Create']">新增</el-button>
               <el-button v-if="scope.row.parentId != 0" link type="primary" icon="Delete" @click="handleDelete(scope.row)" v-hasPermi="['AbpIdentity.OrganizationUnit.Delete']">删除</el-button>
            </template>
         </el-table-column>
      </el-table>

      <!-- 添加或修改部门对话框 -->
      <!-- <el-dialog :title="title" v-model="open" width="600px" append-to-body> -->
              <el-drawer
    ref="drawerRef"
    v-model="open"
    :title="title"
    direction="rtl"
    class="demo-drawer"
  >
         <el-form ref="deptRef" :model="form" :rules="rules" label-width="80px">
            <el-row>
               <el-col :span="24" v-if="form.parentId !== 0">
                  <el-form-item label="上级部门" prop="parentId">
                     <el-tree-select
                        v-model="form.parentId"
                        :data="deptOptions"
                        :props="{ value: 'id', label: 'displayName', children: 'children' }"
                        value-key="id"
                        placeholder="选择上级部门"
                        check-strictly
                     />
                  </el-form-item>
               </el-col>
               <el-col :span="12">
                  <el-form-item label="部门名称" prop="displayName">
                     <el-input v-model="form.displayName" placeholder="请输入部门名称" />
                  </el-form-item>
               </el-col>
            </el-row>
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

<script setup name="Dept">
import { queryOrganizations, getOrganizationExclude } from "@/api/system/organization";
import useDeptStore from '@/store/modules/dept'
import{deepClone}from "@/utils/toolFn"
const { proxy } = getCurrentInstance();
// const { sys_normal_disable } = proxy.useDict("sys_normal_disable");

const deptList = ref([]);
const open = ref(false);
const loading = ref(true);
const showSearch = ref(true);
const title = ref("");
const deptOptions = ref([]);
const isExpandAll = ref(true);
const refreshTable = ref(true);
const deptStore = useDeptStore();
const data = reactive({
  form: {},
  queryParams: {
    name: undefined
  },
  rules: {
    //parentId: [{ required: true, message: "上级部门不能为空", trigger: "blur" }],
    displayName: [{ required: true, message: "部门名称不能为空", trigger: "blur" }],
   //  orderNum: [{ required: true, message: "显示排序不能为空", trigger: "blur" }],
   //  email: [{ type: "email", message: "请输入正确的邮箱地址", trigger: ["blur", "change"] }],
   //  phone: [{ pattern: /^1[3|4|5|6|7|8|9][0-9]\d{8}$/, message: "请输入正确的手机号码", trigger: "blur" }]
  },
});

const { queryParams, form, rules } = toRefs(data);

/** 查询部门列表 */
function getList() {
  loading.value = true;
  queryOrganizations(queryParams.value).then(response => {
    deptList.value = proxy.handleTree(response.items, "id");
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
    parentId: undefined,
    displayName: undefined,
    code: undefined,
    children:[]
  };
  proxy.resetForm("deptRef");
}
/** 搜索按钮操作 */
function handleQuery() {
  getList();
}
/** 重置按钮操作 */
function resetQuery() {
  proxy.resetForm("queryRef");
  handleQuery();
}
/** 新增按钮操作 */
function handleAdd(row) {
  reset();
  queryOrganizations().then(response => {
    deptOptions.value = proxy.handleTree(response.items, "id");
  });
  if (row != undefined) {
    form.value.parentId = row.id;
  }
  open.value = true;
  title.value = "添加部门";
}
/** 展开/折叠操作 */
function toggleExpandAll() {
  refreshTable.value = false;
  isExpandAll.value = !isExpandAll.value;
  nextTick(() => {
    refreshTable.value = true;
  });
}
/** 修改按钮操作 */
function handleUpdate(row) {
  reset();
  getOrganizationExclude(row.id).then(response => {
    deptOptions.value = proxy.handleTree(response.items, "id");
  });
    form.value=deepClone(row);
    open.value = true;
    title.value = "修改部门";
}
/** 提交按钮 */
function submitForm() {
  proxy.$refs["deptRef"].validate(valid => {
    if (valid) {
      if (form.value.id != undefined) {
        deptStore.modifyOrganization(form.value).then(response => {
          proxy.$modal.msgSuccess("修改成功");
          open.value = false;
          getList();
        });
      } else {
        deptStore.addOrganization(form.value).then(response => {
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
  proxy.$modal.confirm('是否确认删除名称为"' + row.displayName + '"的数据项?').then(function() {
    return deptStore.removeOrganization(row.id);
  }).then(() => {
    getList();
    proxy.$modal.msgSuccess("删除成功");
  }).catch(() => {});
}

getList();
</script>
