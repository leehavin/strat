<script setup lang="ts">
import { ref, onBeforeMount } from 'vue';
import { TreeSelect } from 'ant-design-vue';
import { getOrganizationTreeData } from '#/api';

const props = defineProps<{
  modelValue?: number | null;
  placeholder?: string;
}>();

const emits = defineEmits<{
  (e: 'update:modelValue', value: number | null): void;
  (e: 'nodeClick', node: any): void;
}>();

const value = ref(props.modelValue);
const data = ref<any[]>([]);

const convertTreeData = (nodes: any[]): any[] => {
  return nodes.map((node) => ({
    value: node.id,
    title: node.name,
    children: node.children ? convertTreeData(node.children) : undefined,
  }));
};

const handleChange = (val: number) => {
  value.value = val;
  emits('update:modelValue', val);
  emits('nodeClick', { id: val });
};

onBeforeMount(async () => {
  const treeData = await getOrganizationTreeData();
  data.value = convertTreeData(treeData);
});
</script>

<template>
  <TreeSelect
    v-model:value="value"
    tree-default-expand-all
    style="width: 100%"
    :tree-data="data"
    :placeholder="placeholder"
    :field-names="{ children: 'children', label: 'title', value: 'value' }"
    @change="handleChange"
  />
</template>
