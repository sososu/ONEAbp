<template>
    <el-row>
        <el-col :span="8">
            <el-card class="box-card">
                <template #header>
                    <span>应用列表</span>
                </template>
                <!-- 搜索结果 -->
                <div>
                    <div class="list_item flex_space_between" :class="currentSeleteAppName == item.label && 'tree_node_active'" v-for="(item,index) in appList" :key="index" @click="getRelevantAuthList(item)">
                        <span>{{item.label}}</span>
                        <el-icon>
                            <ArrowRight />
                        </el-icon>
                    </div>
                </div>
            </el-card>
        </el-col>
        <el-col class="ml10" :span="15">
            <el-card>
                <template #header>
                    <span>{{currentSeleteAppName}}授权菜单权限</span>
                </template>
                <el-checkbox v-model="menuExpand" @change="handleCheckedTreeExpand($event)">展开/折叠</el-checkbox>
                <el-checkbox v-model="menuNodeAll" @change="handleCheckedTreeNodeAll($event)">全选/全不选</el-checkbox>
                <el-tree ref='authority_tree' :data="menuList" :default-expand-all='true' node-key="code" :props="defaultProps" show-checkbox></el-tree>
                <div class="card_floor_control">
                        <el-button type="primary"
                                   @click="submit">提交</el-button>
                </div>
            </el-card>
        </el-col>
    </el-row>
    </template>
    
<script setup>
 import {
    getSaleVersionApps,
    getSaleVersionMenus,
    SetSaleVersionMenus
} from "@/api/sass/sale";
import {
    getPage
} from "@/api/system/application";
    import {
      nextTick,
        ref
    } from 'vue';
    
    const props = defineProps({
        saleName: {
            type: String,
            require: true
        },
        saleId: {
            type: String,
            require: true
        }
    })
    const { proxy } = getCurrentInstance();
    const defaultProps = {
        children: 'children',
        label: 'label',
    }
    const emits = defineEmits(["update:saleName", "update:saleId"]); 
    const appList = ref([]);
    const menuList = ref([]);
    const authority_tree = ref()
    
    const currentSeleteAppName = ref('');
    const currentSeleteAppId = ref('');
    const menuExpand = ref(true);
    const menuNodeAll = ref(false);
    const start= () =>{
        getAppList();
    }
    
    defineExpose({ start });
    
    function getAppList() {
        getSaleVersionApps(props.saleId).then(response => {
            appList.value = response.items;
            console.log('appList',appList.value)
            if(response.items?.length>0){
                getMenuList(response.items[0])
            }
        })
    }
    
    function getMenuList(app) {
        currentSeleteAppName.value = app.name;
        currentSeleteAppId.value=app.id;
    
        getSaleVersionMenus(props.saleId,app.id).then(response => {
            menuList.value = response.menus;
            nextTick(()=>{
                let checkChildKeys=[];
            response.checkedKeys.forEach(key => {
                if(!authority_tree.value.getNode(key).childNodes ||!authority_tree.value.getNode(key).childNodes.length){
                    checkChildKeys.push(key)
                }
            });
            setCheckedKeys(checkChildKeys);
            })
           
        })
    }
    
    const setCheckedKeys = (checkedKeys) => {
        authority_tree.value.setCheckedKeys(checkedKeys, false)
    }
    const getCheckedKeys = () => {
        return authority_tree.value.getCheckedKeys(false).concat(authority_tree.value.getHalfCheckedKeys());
    }
    function getRelevantAuthList(row) {
        getMenuList(row);
    }
    
    /*提交*/ 
    function submit(){
        var checkedKeys=getCheckedKeys();
        SetSaleVersionMenus(props.saleId,{appId:currentSeleteAppId.value,menuCodes:checkedKeys}).then(response=>{
            proxy.$modal.msgSuccess("新增成功");
        })
    }
    
    
    /** 树权限（展开/折叠）*/
    function handleCheckedTreeExpand(value) {
        let treeList = menuList.value;
        for (let i = 0; i < treeList.length; i++) {
           authority_tree.value.store.nodesMap[treeList[i].code].expanded = value;
        }
    }
    /** 树权限（全选/全不选） */
    function handleCheckedTreeNodeAll(value) {
        authority_tree.value.setCheckedNodes(value ? menuList.value : []);
    }
    
    </script>
    
    <style lang='scss' scoped>
    .list_item {
        padding: 0 0 0 10px;
        line-height: 40px;
        border-left: 3px solid transparent;
        border-bottom: 1px solid #d5d5d5;
        box-sizing: border-box;
    }
    
    .list_item.tree_node_active {
        border-left: 3px solid #0751c1;
    
        .el-icon {
            color: #0751c1;
        }
    }
    
    .list_item:hover {
        opacity: 0.7;
    }
    .card_floor_control{
        text-align: right;
        margin-top: 20px;
    }
    </style>
    