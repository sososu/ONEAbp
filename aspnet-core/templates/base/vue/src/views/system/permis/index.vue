<template>
<div class="app-container">
    <el-tabs v-model="activeName" class="demo-tabs" @tab-click="handleClick">
    <el-tab-pane label="规则" v-hasPermi="['DataPermission.Rule']" name="rule">
        <Rule></Rule>
    </el-tab-pane>
    <el-tab-pane label="用户规则" v-hasPermi="['DataPermission.User']" name="user">
        <UserRule></UserRule>
    </el-tab-pane>
    <el-tab-pane label="数据规则"  v-hasPermi="['DataPermission.Data']" name="data">
        <DataRule></DataRule>
    </el-tab-pane>
  </el-tabs>
</div>
</template>

<script setup name="Permis">
import {
    addApp,
    updateApp,
    deleteApp,
    getPage
} from "@/api/system/application";
import{deepClone}from "@/utils/toolFn"

import DataRule from './components/DataRule'
import UserRule from './components/UserRule'
import Rule from './components/Rule'


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

const activeName = ref('rule')



</script>
