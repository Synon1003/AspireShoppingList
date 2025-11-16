<template>
  <div class="flex justify-center mt-4">
    <button 
      class="join-item btn btn-sm" 
      :disabled="currentPage === 1" 
      @click="emits('loadPage', currentPage - 1)">
        <
    </button>
    <button 
      v-for="page in totalPages" 
      :key="page" 
      class="join-item btn btn-sm"
      :class="{ 'btn-active': page === currentPage }" 
      @click="() => emits('loadPage', page)">
        {{ page }}
    </button>
    <button 
      class="join-item btn btn-sm" 
      :disabled="currentPage == totalPages || totalPages == 0"
       @click="emits('loadPage', currentPage + 1)">
        >
    </button>
  </div>
</template>

<script setup>
const props = defineProps({
  totalPages: {
    type: Number,
    required: true
  },
  currentPage: {
    type: Number,
    required: true
  }
})

const emits = defineEmits(['loadPage'])
</script>