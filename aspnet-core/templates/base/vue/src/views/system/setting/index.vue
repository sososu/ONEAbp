<template>
    <el-card class="full_container">
        <!-- <template #header>
            <div class="flex_space_between">
                <span>{{$router.currentRoute.value.meta && $router.currentRoute.value.meta.title || ''}}</span>
            </div>
        </template> -->
       <div class="settings_manager">
        <div class="settings_nav">
            <ul>
                <li :class="{'active':index === navIndex}" v-for="(item, index) in navList" :key="index" @click="switchSettingsNav(index)">
                    <span>{{item.name}}</span>
                </li>
            </ul>
        </div>

        <el-card class="settings_body" shadow="never">
            <template #header>
                <div class="card-header">
                    <span>{{navList[navIndex].name}}</span>
                </div>
            </template>
            <IdentitySetting v-if="navList[navIndex].name === '身份标识管理'"  ></IdentitySetting>
            <AccountSetting v-if="navList[navIndex].name === '账户管理'"></AccountSetting>
            <FileSetting v-if="navList[navIndex].name === '文件管理'"></FileSetting>

        </el-card>

       </div>
    </el-card>
</template>

<script setup>
import AccountSetting from './components/AccountSetting'
import IdentitySetting from './components/IdentitySetting'
import FileSetting from './components/FileSetting'

import{checkPermi}from "@/utils/permission"
let navList = ref([]),
navIndex  = ref(0)

function getNavList(){
    if(checkPermi(['SettingManagement.IdentitySettings'])){
        navList.value.push({name:'身份标识管理'})
    }
    if(checkPermi(['SettingManagement.AccountSettings'])){
        navList.value.push({name:'账户管理'})
    }
    if(checkPermi(['SettingManagement.FileSettings'])){
        navList.value.push({name:'文件管理'})
    }
}

getNavList();


/* 方法 */
// 切换侧边导航
const switchSettingsNav = index => {
    navIndex.value = index;
}

</script>

<style lang='scss' scoped>
.settings_manager{
    display: flex;
   
    .settings_nav{
        flex-grow: 0;
        flex-shrink: 0;
        flex-basis: 200px;
        margin-right: 10px;
        // background-color: orange;
        ul{
            li{
                font-size: 16px;
                padding: 10px 0;
                border-radius: 4px;
                cursor: pointer;
                span{
                    padding-left: 10px;
                }
            }
            .active{
                background-color: #fedfed;
                color: #f73d93;
                font-weight: 600;
            }
        }
    }
    .settings_body{
        // background-color: red;
        width: 100%;
        height: 100%;
        padding-left: 10px;
    }
}
</style>

