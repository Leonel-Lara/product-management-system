<template>
  <q-dialog
    v-model="showModal"
    @hide="handleClose"
    :position="isMobileScreen ? 'bottom' : 'standard'"
    :full-width="isMobileScreen"
  >
    <q-card class="q-px-md" style="width: 600px; max-width: 80vw">
      <q-card-section class="row items-center q-px-none q-pb-none">
        <div class="text-h6">
          <slot name="title"></slot>
        </div>
        <q-space />
        <q-btn icon="close" flat round dense v-close-popup />
      </q-card-section>

      <q-card-section class="column q-gutter-y-md q-px-none">
        <slot></slot>
      </q-card-section>
      <q-card-actions class="q-px-none">
        <slot name="footer"></slot>
      </q-card-actions>
    </q-card>
  </q-dialog>
</template>
<script setup>
import { ref, watch, computed } from 'vue'
import { useQuasar } from 'quasar'

const emit = defineEmits(['closedByBgCover'])

const handleClose = () => emit('closedByBgCover')

const props = defineProps({
  show: {
    type: Boolean,
    default: false,
  },
})

const $q = useQuasar()
const showModal = ref(false)

watch(
  () => props.show,
  (value) => {
    showModal.value = value
  },
)

const isMobileScreen = computed(() => {
  return $q.screen.lt.md
})
</script>
