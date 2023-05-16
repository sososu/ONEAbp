<template>
<div class="app-container">
   <h2>{{appName}}应用</h2>
    <el-form :model="queryParams" ref="queryRef" :inline="true" v-show="showSearch">
        <el-form-item label="菜单名称" prop="name">
            <el-input v-model="queryParams.name" placeholder="请输入菜单名称" clearable style="width: 200px" @keyup.enter="handleQuery" />
        </el-form-item>
        <el-form-item label="菜单编码" prop="code">
            <el-input v-model="queryParams.code" placeholder="请输入菜单编码" clearable style="width: 200px" @keyup.enter="handleQuery" />
        </el-form-item>
        <el-form-item label="状态" prop="isEnable">
            <el-select v-model="queryParams.isEnable" placeholder="菜单状态" @clear="clearValue" clearable style="width: 200px">
                <el-option v-for="dict in sys_normal_disable" :key="dict.value" :label="dict.label" :value="dict.value" />
            </el-select>
        </el-form-item>
        <el-form-item>
            <el-button type="primary" icon="Search" @click="handleQuery">搜索</el-button>
            <el-button icon="Refresh" @click="resetQuery">重置</el-button>
        </el-form-item>
    </el-form>

    <el-row :gutter="10" class="mb8">
        <el-col :span="1.5">
            <el-button type="primary" plain icon="Plus" @click="handleAdd" v-hasPermi="['system:menu:add']">新增</el-button>
        </el-col>
        <el-col :span="1.5">
            <el-button type="info" plain icon="Sort" @click="toggleExpandAll">展开/折叠</el-button>
        </el-col>
        <right-toolbar v-model:showSearch="showSearch" @queryTable="getList"></right-toolbar>
    </el-row>

    <el-table v-if="refreshTable" v-loading="loading" :data="menuList" row-key="id" :default-expand-all="isExpandAll" :tree-props="{ children: 'children', hasChildren: 'hasChildren' }">
        <el-table-column prop="name" label="菜单名称" :show-overflow-tooltip="true" width="160"></el-table-column>
        <el-table-column prop="icon" label="图标" align="center" width="100">
            <template #default="scope">
                <svg-icon :icon-class="scope.row.icon" />
            </template>
        </el-table-column>
        <el-table-column prop="order" label="排序" width="60"></el-table-column>
        <el-table-column prop="code" label="菜单编码" :show-overflow-tooltip="true"></el-table-column>
        <el-table-column prop="perms" label="权限标识" :show-overflow-tooltip="true"></el-table-column>
        <el-table-column prop="component" label="组件路径" :show-overflow-tooltip="true"></el-table-column>
        <el-table-column prop="isEnable" label="状态" width="80">
            <template #default="scope">
                <dict-tag :options="sys_normal_disable" :value="scope.row.isEnable" />
            </template>
        </el-table-column>
        <el-table-column label="创建时间" align="center" width="160" prop="creationTime">
            <template #default="scope">
                <span>{{ parseTime(scope.row.creationTime) }}</span>
            </template>
        </el-table-column>
        <el-table-column label="操作" align="center" width="210" class-name="small-padding fixed-width">
            <template #default="scope">
                <el-button link type="primary" icon="Edit" @click="handleUpdate(scope.row)" v-hasPermi="['system:menu:edit']">修改</el-button>
                <el-button link type="primary" icon="Plus" @click="handleAdd(scope.row)" v-hasPermi="['system:menu:add']">新增</el-button>
                <el-button link type="primary" icon="Delete" @click="handleDelete(scope.row)" v-hasPermi="['system:menu:remove']">删除</el-button>
            </template>
        </el-table-column>
    </el-table>

    <!-- 添加或修改菜单对话框 -->
    <!-- <el-dialog :title="title" v-model="open" width="680px" append-to-body> -->
             <el-drawer
    ref="drawerRef"
    v-model="open"
    :title="title"
    direction="rtl"
    class="demo-drawer"
  >
        <el-form ref="menuRef" :model="form" :rules="rules" label-width="100px">
            <el-row>
                <el-col :span="24">
                    <el-form-item label="上级菜单">
                        <el-tree-select v-model="form.parentCode" :data="menuOptions" :props="{ value: 'code', label: 'name', children: 'children' }" value-key="id" placeholder="选择上级菜单" check-strictly />
                    </el-form-item>
                </el-col>
                <el-col :span="24">
                    <el-form-item label="菜单类型" prop="menuType">
                        <el-radio-group v-model="form.menuType">
                            <el-radio :label='0'>目录</el-radio>
                            <el-radio :label='1'>菜单</el-radio>
                            <el-radio :label='2'>按钮</el-radio>
                        </el-radio-group>
                    </el-form-item>
                </el-col>
                <el-col :span="24" v-if="form.menuType != 2">
                    <el-form-item label="菜单图标" prop="icon">
                        <el-popover placement="bottom-start" :width="540" v-model:visible="showChooseIcon" trigger="click" @show="showSelectIcon">
                            <template #reference>
                                <el-input v-model="form.icon" placeholder="点击选择图标" @blur="showSelectIcon" v-click-outside="hideSelectIcon" readonly>
                                    <template #prefix>
                                        <svg-icon v-if="form.icon" :icon-class="form.icon" class="el-input__icon" style="height: 32px;width: 16px;" />
                                        <el-icon v-else style="height: 32px;width: 16px;">
                                            <search />
                                        </el-icon>
                                    </template>
                                </el-input>
                            </template>
                            <icon-select ref="iconSelectRef" @selected="selected" />
                        </el-popover>
                    </el-form-item>
                </el-col>
                <el-col :span="12">
                    <el-form-item label="菜单名称" prop="name">
                        <el-input v-model="form.name" placeholder="请输入菜单名称" />
                    </el-form-item>
                </el-col>
                <el-col :span="12">
                    <el-form-item label="菜单编码" prop="code">
                        <el-input v-model="form.code" placeholder="请输入菜单编码" />
                    </el-form-item>
                </el-col>
                <el-col :span="12">
                    <el-form-item label="显示排序" prop="order">
                        <el-input-number v-model="form.order" controls-position="right" :min="0" />
                    </el-form-item>
                </el-col>
                <el-col :span="12" v-if="form.menuType != 2">
                    <el-form-item>
                        <template #label>
                            <span>
                                <el-tooltip content="选择是外链则路由地址需要以`http(s)://`开头" placement="top">
                                    <el-icon>
                                        <question-filled />
                                    </el-icon>
                                </el-tooltip>是否外链
                            </span>
                        </template>
                        <el-radio-group v-model="form.isFrame">
                            <el-radio :label='true'>是</el-radio>
                            <el-radio :label='false'>否</el-radio>
                        </el-radio-group>
                    </el-form-item>
                </el-col>
                <el-col :span="12" v-if="form.menuType != 2">
                    <el-form-item prop="path">
                        <template #label>
                            <span>
                                <el-tooltip content="访问的路由地址，如：`user`，如外网地址需内链访问则以`http(s)://`开头" placement="top">
                                    <el-icon>
                                        <question-filled />
                                    </el-icon>
                                </el-tooltip>
                                路由地址
                            </span>
                        </template>
                        <el-input v-model="form.path" placeholder="请输入路由地址" />
                    </el-form-item>
                </el-col>
                <el-col :span="12" v-if="form.menuType == 1">
                    <el-form-item prop="component">
                        <template #label>
                            <span>
                                <el-tooltip content="访问的组件路径，如：`system/user/index`，默认在`views`目录下" placement="top">
                                    <el-icon>
                                        <question-filled />
                                    </el-icon>
                                </el-tooltip>
                                组件路径
                            </span>
                        </template>
                        <el-input v-model="form.component" placeholder="请输入组件路径" />
                    </el-form-item>
                </el-col>
                <el-col :span="12" v-if="form.menuType != 0">
                    <el-form-item>
                        <!-- <el-input v-model="form.perms" placeholder="请输入权限标识" maxlength="100" /> -->
                         <el-select
        v-model="form.perms"
        filterable
        clearable
        remote
        reserve-keyword
        placeholder="请输入权限字符"
        :remote-method="remoteMethod"
        :loading="loading"
      >
        <el-option
          v-for="item in permsLables"
          :key="item.value"
          :label="item.label"
          :value="item.value"
        />
      </el-select>
                        <template #label>
                            <span>
                                <el-tooltip content="控制器中定义的权限字符，如：@PreAuthorize(`@ss.hasPermi('system:user:list')`)" placement="top">
                                    <el-icon>
                                        <question-filled />
                                    </el-icon>
                                </el-tooltip>
                                权限字符
                            </span>
                        </template>
                    </el-form-item>
                </el-col>
                <el-col :span="12" v-if="form.menuType == 1">
                    <el-form-item>
                        <el-input v-model="form.query" placeholder="请输入路由参数" maxlength="255" />
                        <template #label>
                            <span>
                                <el-tooltip content='访问路由的默认传递参数，如：`{"id": 1, "name": "ry"}`' placement="top">
                                    <el-icon>
                                        <question-filled />
                                    </el-icon>
                                </el-tooltip>
                                路由参数
                            </span>
                        </template>
                    </el-form-item>
                </el-col>
                <el-col :span="12" v-if="form.menuType == 1">
                    <el-form-item>
                        <template #label>
                            <span>
                                <el-tooltip content="选择是则会被`keep-alive`缓存，需要匹配组件的`name`和地址保持一致" placement="top">
                                    <el-icon>
                                        <question-filled />
                                    </el-icon>
                                </el-tooltip>
                                是否缓存
                            </span>
                        </template>
                        <el-radio-group v-model="form.isCache">
                            <el-radio :label="true">缓存</el-radio>
                            <el-radio :label="false">不缓存</el-radio>
                        </el-radio-group>
                    </el-form-item>
                </el-col>
                <el-col :span="12" v-if="form.menuType != 2">
                    <el-form-item>
                        <template #label>
                            <span>
                                <el-tooltip content="选择隐藏则路由将不会出现在侧边栏，但仍然可以访问" placement="top">
                                    <el-icon>
                                        <question-filled />
                                    </el-icon>
                                </el-tooltip>
                                显示状态
                            </span>
                        </template>
                        <el-radio-group v-model="form.visible">
                            <el-radio v-for="dict in sys_show_hide" :key="dict.value" :label="dict.value">{{ dict.label }}</el-radio>
                        </el-radio-group>
                    </el-form-item>
                </el-col>
                <el-col :span="12" v-if="form.menuType != 2">
                    <el-form-item>
                        <template #label>
                            <span>
                                <el-tooltip content="选择停用则路由将不会出现在侧边栏，也不能被访问" placement="top">
                                    <el-icon>
                                        <question-filled />
                                    </el-icon>
                                </el-tooltip>
                                菜单状态
                            </span>
                        </template>
                        <el-radio-group v-model="form.isEnable">
                            <el-radio v-for="dict in sys_normal_disable" :key="dict.value" :label="dict.value">{{ dict.label }}</el-radio>
                        </el-radio-group>
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

<script setup name="Menu">
import {
    addMenu,
    delMenu,
    getMenu,
    listMenu,
    updateMenu
} from "@/api/system/menu";
import {
    getPerms
} from "@/api/role";
import SvgIcon from "@/components/SvgIcon";
import IconSelect from "@/components/IconSelect";
import {
    ClickOutside as vClickOutside
} from 'element-plus'
const route = useRoute();
const {
    proxy
} = getCurrentInstance();
// const {
//     sys_show_hide,
//     sys_normal_disable
// } = proxy.useDict("sys_show_hide", "sys_normal_disable");

const sys_normal_disable = [{
    label: '启用',
    value: true
}, {
    label: '禁用',
    value: false
}];
const sys_show_hide = [{
    label: '显示',
    value: true
}, {
    label: '隐藏',
    value: false
}];

const menuList = ref([]);
const open = ref(false);
const loading = ref(true);
const showSearch = ref(true);
const title = ref("");
const menuOptions = ref([]);
const isExpandAll = ref(false);
const refreshTable = ref(true);
const showChooseIcon = ref(false);
const iconSelectRef = ref(null);
const permsLables=ref([]);
let appId = route.query && route.query.id;
let appName = route.query && route.query.name;
console.log("appid,appname",appId,appName)
const data = reactive({
    form: {},
    queryParams: {
        name: undefined,
        code: undefined,
        isEnable: undefined,
        sysAppId: appId,
        pageIndex: 1,
        pageSize: 1000
    },
    rules: {
        name: [{
            required: true,
            message: "菜单名称不能为空",
            trigger: "blur"
        }],
        order: [{
            required: true,
            message: "菜单顺序不能为空",
            trigger: "blur"
        }],
        path: [{
            required: true,
            message: "路由地址不能为空",
            trigger: "blur"
        }]
    },
});

const {
    queryParams,
    form,
    rules
} = toRefs(data);

/** 查询菜单列表 */
function getList() {
    loading.value = true;
    listMenu(queryParams.value).then(response => {
        console.log("menus:", response);
        menuList.value = proxy.handleTree(response.items||[], "code", "parentCode");
        loading.value = false;
    });
}
/** 查询菜单下拉树结构 */
function getTreeselect() {
    menuOptions.value = [];
    listMenu(queryParams.value).then(response => {
        const menu = {
            id: 0,
            code: "",
            name: "主类目",
            children: []
        };
        menu.children = proxy.handleTree(response.items, "code", "parentCode");
        menuOptions.value.push(menu);
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
        sysAppId: appId,
        id: undefined,
        code: undefined,
        parentCode: undefined,
        name: undefined,
        icon: undefined,
        menuType: 0,
        order: 0,
        isFrame: false,
        isCache: false,
        visible: true,
        isEnable: true
    };
    proxy.resetForm("menuRef");
}
/** 展示下拉图标 */
function showSelectIcon() {
    iconSelectRef.value.reset();
    showChooseIcon.value = true;
}
/** 选择图标 */
function selected(name) {
    form.value.icon = name;
    showChooseIcon.value = false;
}
/** 图标外层点击隐藏下拉列表 */
function hideSelectIcon(event) {
    var elem = event.relatedTarget || event.srcElement || event.target || event.currentTarget;
    var className = elem.className;
    if (className !== "el-input__inner") {
        showChooseIcon.value = false;
    }
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
    console.log("add form",form);
    getTreeselect();
    if (row != null && row.code) {
        form.value.parentCode = row.code;
    } else {
        form.value.parentCode = '';
    }
    open.value = true;
    title.value = "添加菜单";
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
async function handleUpdate(row) {
    reset();
    await getTreeselect();
    getMenu(row.id).then(response => {
        form.value = response;
        open.value = true;
        title.value = "修改菜单";
    }).catch(error=>{
        console.log("getMenu error",error)
    });
}
/** 提交按钮 */
function submitForm() {
    proxy.$refs["menuRef"].validate(valid => {
        if (valid) {
            if (form.value.id != undefined) {
                updateMenu(form.value.id,form.value).then(response => {
                    proxy.$modal.msgSuccess("修改成功");
                    open.value = false;
                    getList();
                });
            } else {
                addMenu(form.value).then(response => {
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
    proxy.$modal.confirm('是否确认删除名称为"' + row.name + '"的数据项?').then(function () {
        return delMenu(row.id);
    }).then(() => {
        getList();
        proxy.$modal.msgSuccess("删除成功");
    }).catch(() => {});
}

function remoteMethod(query) {
  if (query) {
    loading.value = true
    setTimeout(() => {
      loading.value = false
      permsLables.value = premslist.filter((item) => {
        return item.label.toLowerCase().includes(query.toLowerCase())
      })
    }, 200)
  } else {
    permsLables.value = []
  }
}

function clearValue(val){
   queryParams.value.isEnable=undefined;
}

let premslist=[];
function getPermiss(){
   getPerms().then(response => {
     premslist=response.items.map((item) => {
    return { value: item, label: item }
  })
    });
}

getPermiss();
getList();
</script>
