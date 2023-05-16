<template>
      <div class="app-container">
        <h2 class="title">通用设置</h2>
        <el-form ref="appRef" :model="form" :rules="rules"  label-position="top">
            <el-form-item  prop="requiredLength">
                <el-checkbox v-model="form.isSelfRegistrationEnabled" label="是否启用注册" />
            </el-form-item>
        </el-form>
        <div class="dialog-footer">
                <el-button type="primary" @click="submitForm" v-hasPermi="['SettingManagement.AccountSettings.Update']">保存</el-button>
        </div>
    </div>
</template>
<script setup>
import {
    getAccountSetting,
    setAccountSetting
} from "@/api/system/setting";
import{deepClone}from "@/utils/toolFn"

const { proxy } = getCurrentInstance();
const open = ref(false);
const loading = ref(true);

const data = reactive({
    form: {},
    rules: {
    },
});
const {
    form,
    rules
} = toRefs(data);


/** 查询应用列表 */
function getSetting() {
    loading.value = true;
    getAccountSetting().then(response => {
        form.value = response;
        loading.value = false;
    });
}

/** 提交按钮 */
function submitForm() {
    proxy.$refs["appRef"].validate(valid => {
        if (valid) {
            setAccountSetting(form.value).then(response => {
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