<template>
<div class="app-container">
<el-card class="box-card" > 
    <template #header>
      <div class="card-header">
        <span>存储空间使用情况统计</span>
      </div>
    </template>
    <el-row justify="center">
    <el-col :span="2"  v-for="(detail, index) in statistics.fileTypeDetailStatistics" :key="index">
      <el-statistic :title="detail.fileType" :value="detail.totalCount" />
    </el-col>
    <el-col :span="2">
      <el-statistic title="总用容量(Mb)" :value="Math.ceil(statistics.totalSize/1024/1024)" >
        <template #suffix>
           
             <!-- <el-progress :percentage="percentage" :color="customColors" /> -->
        </template>
      </el-statistic>
    </el-col>
    <el-col :span="6" style="text-align: center;">
      <el-statistic :value="Math.ceil(statistics.useSize/1024/1024)" :formatter="v=>''" >
        <template #title>
            <div style="display: inline-flex; align-items: left;">
              <span>存储空间</span>
              <span style="margin-left: 24px; color: grey;">已使用{{Math.ceil(statistics.useSize/1024/1024) }} MB/{{ Math.ceil(statistics.totalSize/1024/1024) }} MB</span>
            </div>
          </template>
        <template #prefix>
           <!-- <el-progress type="dashboard" :percentage="percentage" :color="customColors">
                    <template #default="{ percentage }">
                    <span class="percentage-value">{{ percentage }}%</span>
                    <span class="percentage-label">总{{Math.ceil(statistics.totalSize/1024/1024/1024)}}MB</span>
                    </template>
             </el-progress>--> 
             <div style="width: 220px"><el-progress type="line" :percentage="percentage" :color="customColors"  :text-inside="true" :stroke-width="20"/></div>
    
        </template>
       
      </el-statistic>
    </el-col>
  </el-row>
</el-card>

    <el-form :model="queryParams" ref="queryRef" :inline="true" v-show="showSearch" label-width="68px">
        <el-form-item label="文件名" prop="appName">
            <el-input v-model="queryParams.originalFileName" placeholder="请输入文件名或真实文件名称" clearable style="width: 240px" @keyup.enter="handleQuery" />
        </el-form-item>
    
        <el-form-item>
            <el-button type="primary" icon="Search" @click="handleQuery">搜索</el-button>
            <el-button icon="Refresh" @click="resetQuery">重置</el-button>
        </el-form-item>
    </el-form>

    <el-row :gutter="10" class="mb8">
        <!-- <el-col :span="1.5">
            <el-button type="primary" plain icon="Plus" @click="handleAdd" v-hasPermi="['SysResource.App.Create']">新增</el-button>
        </el-col> -->
        <right-toolbar v-model:showSearch="showSearch" @queryTable="getList"></right-toolbar>
    </el-row>

    <el-table v-loading="loading" :data="typeList" @selection-change="handleSelectionChange">
        <el-table-column type="selection" width="55" align="center" />
        <el-table-column label="文件名称" align="center" prop="originalFileName" width="180">
            <template #default="scope">
                <div v-if="scope.row.fileType=='图片'">
                    <AuthImg :authSrc="`${baseUrl}/api/file-management/file/stream/${scope.row.fileName}`" class="user-avatar" style="width: 50px;height: 50px;"/>
                </div>
                <div v-else-if="scope.row.fileType=='音频'">
                    <img :src="musicLogo" class="user-avatar" style="width: 50px;height: 50px;"/>
                </div>
                <div v-else-if="scope.row.fileType=='视频'">
                    <img :src="videoLogo" class="user-avatar" style="width: 50px;height: 50px;"/>
                </div>
                <div v-else-if="scope.row.fileType=='文本'">
                    <img :src="textLogo" class="user-avatar" style="width: 50px;height: 50px;"/>
                </div>
                <div v-else-if="scope.row.fileType=='文档'">  
                    <img :src="wordLogo" class="user-avatar" style="width: 50px;height: 50px;"/>
                </div>
                <div v-else-if="scope.row.fileType=='未知'">
                    <img :src="unknowLogo" class="user-avatar" style="width: 50px;height: 50px;"/>
                </div>
                <div><span>{{ scope.row.originalFileName }}</span></div>
            </template>
        </el-table-column>
        <el-table-column label="真实文件名称" align="center" prop="fileName" />
        <el-table-column label="文件类型" align="center" prop="fileType" />
        <el-table-column label="文件大小" align="center" prop="fileSize" />
        <el-table-column label="文件标签" align="center" prop="tag" />
        <el-table-column label="创建时间" align="center" prop="creationTime" width="180">
            <template #default="scope">
                <span>{{ parseTime(scope.row.creationTime) }}</span>
            </template>
        </el-table-column>
        <el-table-column label="操作" align="center" width="220" class-name="small-padding fixed-width">
            <template #default="scope">
                <el-button link type="primary" icon="Delete" @click="handleDelete(scope.row)" v-hasPermi="['FileManagementFile.Delete']">删除</el-button>
                <el-button link type="primary" icon="download" @click="handDownload(scope.row)" v-hasPermi="['FileManagementFile.Download']">下载</el-button>
            </template>
        </el-table-column>
    </el-table>

    <pagination v-show="total > 0" :total="total" v-model:page="queryParams.pageIndex" v-model:limit="queryParams.pageSize" @pagination="getList" />

</div>
</template>

<script setup name="File">
import musicLogo from '@/assets/images/music.jpg'
import textLogo from '@/assets/images/text.jpg'
import videoLogo from '@/assets/images/video.jpg'
import wordLogo from '@/assets/images/word.jpg'
import unknowLogo from '@/assets/images/unknow.jpg'

import {
    getFilePage,
    deleteFile,
    downloadFile,
    getStatistics
} from "@/api/file-management/file";
import { download } from "@/utils/request";
import{deepClone}from "@/utils/toolFn"
import { ref } from "vue";

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
const percentage=ref(0);
const data = reactive({
    form: {},
    statistics:{},
    queryParams: {
        pageIndex: 1,
        pageSize: 10,
        originalFileName: undefined,
        fileName: undefined
    },
    rules: {
    },
});

const {
    queryParams,
    form,
    statistics,
    rules
} = toRefs(data);

const baseUrl=import.meta.env.VITE_APP_BASE_API;

const customColors = [
  { color: '#f56c6c', percentage: 100 },
  { color: '#e6a23c', percentage: 80 },
  { color: '#5cb87a', percentage: 60 },
  { color: '#1989fa', percentage: 40 },
  { color: '#6f7ad3', percentage: 20 },
]


function getStatisticsInfo(){
    getStatistics().then(res=>{
        statistics.value=res;
        percentage.value=Math.ceil(res.useSize/res.totalSize*100);
    })
}

function handDownload(data){
    var ofileName=data.originalFileName;
    if(!ofileName.includes(".")){
        var files= data.fileName.split('.');
        ofileName=ofileName+'.'+files[files.length-1]
    }

    downloadFile(data.fileName,ofileName).then(res=>{

    })
}

/** 查询应用列表 */
function getList() {
    loading.value = true;
    getFilePage(queryParams.value).then(response => {
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
        appName: undefined,
        appCode: undefined,
        appVersion: undefined,
        description: undefined,
        appUrl: undefined
    };
    proxy.resetForm("appRef");
}
/** 搜索按钮操作 */
function handleQuery() {
    queryParams.value.pageIndex = 1;
    queryParams.value.fileName =  queryParams.value.originalFileName;

    getList();
}
/** 重置按钮操作 */
function resetQuery() {
    dateRange.value = [];
    proxy.resetForm("queryRef");
    handleQuery();
}

/** 多选框选中数据 */
function handleSelectionChange(selection) {
    ids.value = selection.map(item => item.id);

    console.log("ids",ids.value);

    single.value = selection.length != 1;
    multiple.value = !selection.length;
}

/** 删除按钮操作 */
function handleDelete(row) {
    const ids = row.id || ids.value;
    proxy.$modal.confirm('是否确认删除文件为"' + row.originalFileName + '"的数据项？').then(function () {
        return deleteFile(row.fileName);
    }).then(() => {
        getList();
        proxy.$modal.msgSuccess("删除成功");
    }).catch(() => {});
}
getStatisticsInfo();
getList();
</script>
<style lang="scss">

.box-card{
    // .el-col{
    //    text-align: center;
    // }
    margin-bottom: 20px;
}


.percentage-value {
    display: block;
    margin-top: 10px;
    font-size: 28px;
  }
  .percentage-label {
    display: block;
    margin-top: 10px;
    font-size: 12px;
  }

</style>
