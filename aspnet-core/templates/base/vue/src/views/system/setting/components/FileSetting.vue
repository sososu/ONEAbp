<template>
      <div class="app-container">
        <h2 class="title">上传文件设置</h2>
        <el-form ref="appRef" :model="form" :rules="rules"  label-position="top">
            <el-form-item label="总容量（字节）" prop="totalLimitSize">
                <el-input v-model.trim="form.totalLimitSize" type="number" :min="104857600" step="10240" placeholder="请输入" />
                <span class="tips">字节单位</span>
            </el-form-item>
            <el-form-item label="允许上传单个文件最大容量（字节）" prop="limitSize">
                <el-input v-model.trim="form.limitSize" type="number" :min="1024" step="1024" placeholder="请输入" />
                <span class="tips">字节单位</span>
            </el-form-item>
            <el-form-item label="允许上传的文件格式(逗号隔开)" prop="supportMimeType">
                <el-input v-model.trim="form.supportMimeType" type="textarea" placeholder="请输入" />
                <span class="tips">值是MIME类型</span>
            </el-form-item>

        </el-form>
        <div class="dialog-footer">
                <el-button type="primary" @click="submitForm" v-hasPermi="['SettingManagement.FileSettings.Update']">保存</el-button>
        </div>
    </div>
</template>
<script setup>
import {
    getFileSetting,
    setFileSetting
} from "@/api/system/setting";
import{deepClone}from "@/utils/toolFn"

const { proxy } = getCurrentInstance();
const open = ref(false);
const loading = ref(true);

const data = reactive({
    form: {},
    rules: {
        'supportMimeType': [{
            required: true,
            message: "允许上传的文件格式不能为空",
            trigger: "blur"
        }]
    },
});
const {
    form,
    rules
} = toRefs(data);


/** 查询应用列表 */
function getSetting() {
    loading.value = true;
    getFileSetting().then(response => {
        form.value = response;
        loading.value = false;
    });
}

/** 提交按钮 */
function submitForm() {
    proxy.$refs["appRef"].validate(valid => {
        if (valid) {
            setFileSetting(form.value).then(response => {
                    proxy.$modal.msgSuccess("提交成功");
                });
        }
    });
}

getSetting();

</script>
<style lang='scss' scoped>

.app-container{
    .title{
        margin-top: -10px;
      }
        .tips{
                    color: #9dabb6;
                    font-size: 12px;
                    margin-left: 10px;
                }
}
</style>