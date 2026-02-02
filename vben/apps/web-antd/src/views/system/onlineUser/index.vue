<script lang="ts" setup>
import { h, reactive, ref, onMounted } from 'vue';
import { type VxeGridProps } from 'vxe-table';
import { VxeButton } from 'vxe-pc-ui';
import { useOnlineUserStore } from '#/store';
import { $t } from '@vben/locales';
import { Card, message } from 'ant-design-vue';
import { useAccess } from '@vben/access';
const { hasAccessByCodes } = useAccess();

const formRef = ref();

const handleInitialFormParams = () => ({
  name: '',
});
const formItems = [
  {
    field: 'name',
    title: $t('onlineUser.search.name'),
    span: 6,
    itemRender: {
      name: '$input',
      props: { placeholder: $t('onlineUser.search.name') },
    },
  },
  {
    span: 6,
    itemRender: {
      name: '$buttons',
      children: [
        {
          props: {
            type: 'submit',
            icon: 'vxe-icon-search',
            content: $t('common.search'),
            status: 'primary',
          },
        },
        {
          props: {
            type: 'reset',
            icon: 'vxe-icon-undo',
            content: $t('common.reset'),
          },
        },
      ],
    },
  },
];
const formData = reactive<{ name: string }>(handleInitialFormParams());

const handleSearch = () => {
  connection?.invoke('GetOnlineUsers');
};
interface OnlineUser {
  connectionId: string;
  userId: string;
  userName: string;
  connectedTime: string;
  ip: string;
  ipString: string;
}
const tableData = ref<OnlineUser[]>();
const gridOptions = reactive<VxeGridProps<OnlineUser>>({
  border: false,
  round: true,
  resizable: true,
  maxHeight: 650,
  minHeight: 650,
  align: null,
  columns: [
    { type: 'seq', width: 100, align: 'center' },
    {
      field: 'connectionId',
      visible: false,
      title: $t('onlineUser.columns.connectionId'),
    },
    { field: 'userName', title: $t('onlineUser.columns.userName') },
    { field: 'connectedTime', title: $t('onlineUser.columns.connectedTime') },
    { field: 'ip', title: $t('onlineUser.columns.ip') },
    { field: 'ipString', title: $t('onlineUser.columns.ipStr') },
    {
      field: 'operate',
      title: $t('common.operation'),
      align: 'center',
      slots: {
        default: ({ data, row }) => [
          h(`p`, {}, [
            hasAccessByCodes(['system.onlineuser.logout'])
              ? h(
                  VxeButton,
                  {
                    style: {
                      display:
                        row.connectionId == connection?.connectionId
                          ? 'none'
                          : 'inline',
                    },
                    status: 'danger',
                    mode: `text`,
                    onClick() {
                      connection?.invoke('OfflineUser', row.connectionId);
                    },
                  },
                  () => $t('onlineUser.offline'),
                )
              : null,
            hasAccessByCodes(['system.onlineuser.sendmsg'])
              ? h(
                  VxeButton,
                  {
                    style: {
                      marginLeft: `10px`,
                      display:
                        row.connectionId == connection?.connectionId
                          ? 'none'
                          : 'inline',
                    },
                    mode: `text`,
                    status: 'primary',
                    onClick() {
                      messageOptions.modalValue = true;
                      messageOptions.connectionId = row.connectionId;
                    },
                  },
                  () => $t('onlineUser.sendMessage'),
                )
              : null,
          ]),
        ],
      },
    },
  ],
});
const onlineUserStore = useOnlineUserStore();
const connection = onlineUserStore.connection;

connection?.on('UpdateUser', (result: OnlineUser[]) => {
  result = result.filter(
    (item) => item.connectionId != connection.connectionId,
  );
  if (formData.name) {
    tableData.value = result.filter((item) =>
      item.userName.includes(formData.name),
    );
    return false;
  }
  tableData.value = result;
});

const messageOptions = reactive({
  connectionId: '',
  modalValue: false,
  message: '',
});

const handleSendMessage = () => {
  connection?.invoke(
    'SendMessage',
    messageOptions.connectionId,
    messageOptions.message,
  );
  messageOptions.modalValue = false;
  messageOptions.message = '';
  message.success('消息发送成功');
};
onMounted(() => {
  if (connection?.state == `Connected`) {
    connection.invoke('GetOnlineUsers');
  }
});
</script>
<template>
  <re-page>
    <Card :bordered="false">
      <vxe-form
        ref="formRef"
        :data="formData"
        :items="formItems"
        @submit="handleSearch"
        @reset="handleInitialFormParams"
      />
    </Card>
    <Card :bordered="false" class="table-card">
      <vxe-grid v-bind="gridOptions" :data="tableData" />
    </Card>
    <vxe-modal v-model="messageOptions.modalValue" show-footer>
      <template #title>
        <span style="color: red">发送消息</span>
      </template>
      <template #default>
        <vxe-textarea
          v-model="messageOptions.message"
          :rows="5"
          :maxlength="120"
          :showWordCount="true"
          placeholder="请输入要发送的消息"
        />
      </template>
      <template #footer>
        <vxe-button
          status="primary"
          content="确定"
          @click="handleSendMessage"
        />
      </template>
    </vxe-modal>
  </re-page>
</template>

<style scoped>
.table-card {
  margin-top: 10px;
}
</style>
