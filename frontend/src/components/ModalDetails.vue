<template>
  <q-dialog
    v-model="showModal"
    @hide="handleClose"
    :position="isMobileScreen ? 'bottom' : 'standard'"
    :full-width="isMobileScreen"
  >
    <q-card flat style="width: 400px; max-width: 60vw">
      <div class="row justify-center items-center bg-grey-5 full-width q-py-xl">
        <q-icon name="bi-card-image" size="lg" color="grey-8" />
      </div>

      <q-card-section>
        <div class="row no-wrap items-center">
          <div class="col text-h6 ellipsis">{{ productObj.name }}</div>
        </div>

        <q-rating v-model="randomNumber" :max="5" size="32px" />
      </q-card-section>

      <q-card-section class="q-pt-none">
        <div class="text-subtitle1">{{ generalUtil.formatMoney(productObj.price) }}</div>
        <div class="text-caption text-grey">
          {{ productObj.category }}
        </div>
      </q-card-section>

      <q-separator />

      <q-card-actions>
        <q-btn color="primary" icon="bi-bag" no-caps padding="sm md" label="Comprar" />
      </q-card-actions>
    </q-card>
  </q-dialog>
</template>
<script setup>
import { ref, watch, computed } from 'vue'
import { useQuasar } from 'quasar'
import productApi from 'src/api/productApi'
import generalUtil from 'src/utils/general'

const emit = defineEmits(['close'])

const handleClose = () => emit('close')

const props = defineProps({
  show: {
    type: Boolean,
    default: false,
  },
  objId: {
    type: Number,
    default: 0,
  },
})

const $q = useQuasar()
const loading = ref(false)
const showModal = ref(false)
const randomNumber = ref(0)
const productObj = ref({ name: '', category: '', price: '' })

watch(
  () => props.show,
  (value) => {
    showModal.value = value
  },
)
watch(
  () => props.objId,
  (value) => {
    if (value) fetchProduct(value)
  },
)

const fetchProduct = async (objId) => {
  loading.value = true
  try {
    const { data } = await productApi.getDetails(objId)
    productObj.value = data
    randomNumber.value = Math.floor(Math.random() * 6)
  } finally {
    loading.value = false
  }
}

const isMobileScreen = computed(() => {
  return $q.screen.lt.md
})
</script>
