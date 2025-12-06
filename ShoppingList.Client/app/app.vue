<template>
  <div class="max-w-4xl mx-auto px-4 py-10">
    <Header text="Shopping List" />

    <div class="flex items-center justify-between mb-4">
      <CountIndicator :totalCount="totalCount" />
      <AddModal @addItem="addItem" />
    </div>

    <div v-if="!loading" class="overflow-x-auto mx-auto max-w-4xl w-full">
      <ItemTable
        :items="tableItems"
        :currentPage="currentPage"
        :pageSize="pageSize"
        @toggleItemStatus="toggleItemStatus"
        @deleteItem="deleteItemById" />

      <Pagination
        :totalPages="totalPages"
        :currentPage="currentPage"
        @loadPage="loadPage" />
    </div>

    <Toast
      v-if="showToast"
      :type="toastType" 
      :message="toastMessage"
    />
    <Loader :isLoading="loading"/>
  </div>
</template>

<script setup>
import { ref, computed, onMounted } from 'vue'

import env from '~/lib/env'
import { itemService } from '~/composables/useItemService'

import Header from './components/header.vue'
import CountIndicator from './components/countIndicator.vue'
import AddModal from './components/addModal.vue'
import ItemTable from './components/itemTable.vue'
import Pagination from './components/pagination.vue'
import Toast from './components/toast.vue'
import Loader from './components/loader.vue'

let tableItems = ref([])
let totalItems = ref([])
const totalCount = ref(0)

const pageSize = env.PAGE_SIZE
const totalPages = computed(() => Math.ceil(totalItems.value.length / pageSize))
const currentPage = ref(1)

const showToast = ref(false)
const toastType = ref('')
const toastMessage = ref('')
const loading = ref(false)


async function loadItems()
{
  loading.value = true
  const [fetchedItems, message] = await itemService.fetchItems()
  if (message != '') {
    showToastMessage('error', message)
  }
  else {
    totalItems.value = fetchedItems
    totalCount.value = totalItems.value.length
    if (totalCount.value === 0)
      showToastMessage('info', 'There are no items in the shopping list.')
  }
  loading.value = false
}

async function switchItemById(itemId)
{
  const [fetchedItem, message] = await itemService.fetchItemById(itemId)
  if (fetchedItem != null) {
    const totalIndex = totalItems.value.findIndex(item => item.id === itemId)
    const tableIndex = tableItems.value.findIndex(item => item.id === itemId)
    if (totalIndex !== -1 && tableIndex !== -1)
    {
      totalItems.value[totalIndex] = fetchedItem
      tableItems.value[tableIndex] = fetchedItem
    }
  } else {
    showToastMessage('error', message)
  }
}

async function addItem(item)
{
  const [isSuccess, message] = await itemService.createItem(item)
  if (isSuccess) {
    showToastMessage('success', message)
    await loadItems()
    loadPage(currentPage.value)
  } else {
    showToastMessage('error', message)
  }
}

async function toggleItemStatus(itemId)
{
  const [isSuccess, message] = await itemService.patchItem(itemId)
  if (isSuccess) {
    showToastMessage('success', message)
    await switchItemById(itemId)
  } else {
    showToastMessage('error', message)
  }
}

async function deleteItemById(itemId)
{
  const [isSuccess, message] = await itemService.deleteItem(itemId)
  if (isSuccess) {
    showToastMessage('success', message)
    await loadItems()
    loadPage(currentPage.value)
  } else {
    showToastMessage('error', message)
  }
}

function showToastMessage(type, message) {
  toastType.value = type
  toastMessage.value = message
  showToast.value = true
  setTimeout(() => { showToast.value = false }, 5000)
}

async function loadPage(page = 1)
{
  if (page < 1) return
  if (0 < totalPages.value && totalPages.value < page)
    page = totalPages.value

  currentPage.value = page

  const start = (currentPage.value - 1) * pageSize
  const end = start + pageSize
  tableItems.value = totalItems.value.slice(start, end)
}

onMounted(async () =>
{
  await loadItems()
  loadPage(1)
})

</script>

<style scoped></style>