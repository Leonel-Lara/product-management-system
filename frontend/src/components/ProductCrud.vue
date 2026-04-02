<template>
  <ModalCrud :show="props.show" @closedByBgCover="fnClosedByBgCover">
    <template v-if="productObj.id" v-slot:title>Editar produto</template>
    <template v-else v-slot:title>Criar novo produto</template>

    <q-input color="primary" outlined v-model.trim="productObj.sku" label="SKU *">
      <template v-slot:prepend>
        <q-icon name="bi-hash" size="sm" />
      </template>
    </q-input>
    <div class="row q-gutter-x-lg">
      <q-input class="col" color="primary" outlined v-model.trim="productObj.name" label="Nome *">
        <template v-slot:prepend>
          <q-icon name="bi-box-seam" size="sm" />
        </template>
      </q-input>
      <q-select
        class="col"
        color="primary"
        outlined
        v-model.trim="productObj.category"
        label="Categoria *"
        :options="categoryOptions"
      >
        <template v-slot:prepend>
          <q-icon name="bi-tag" size="sm" />
        </template>
      </q-select>
    </div>
    <div class="row q-gutter-x-lg">
      <q-input
        class="col"
        color="primary"
        type="number"
        outlined
        v-model.number="productObj.price"
        label="Preço *"
      >
        <template v-slot:prepend>
          <q-icon name="bi-currency-dollar" size="sm" />
        </template>
      </q-input>
      <q-input
        class="col"
        color="primary"
        type="number"
        outlined
        v-model.number="productObj.stockQuantity"
        label="Qntd. Estoque *"
      >
        <template v-slot:prepend>
          <q-icon name="bi-boxes" size="sm" />
        </template>
      </q-input>
    </div>
    <template v-slot:footer>
      <div class="col-12 row justify-end">
        <q-btn
          @click="emit('close')"
          label="Cancelar"
          color="grey-6"
          no-caps
          outline
          padding="sm md"
        />
        <q-btn
          @click="checkProductFields"
          class="q-ml-lg"
          label="Salvar"
          color="primary"
          no-caps
          padding="sm md"
          :loading="props.saving"
        />
      </div>
    </template>
  </ModalCrud>
</template>
<script setup>
import { ref, watch } from 'vue'
import { useQuasar } from 'quasar'
import ModalCrud from './ModalCrud.vue'
import notify from 'src/utils/notifications'

const $q = useQuasar()

const emit = defineEmits(['save', 'close', 'delete'])

const props = defineProps({
  obj: {
    type: Object,
    default: () => ({}),
  },
  show: {
    type: Boolean,
    default: false,
  },
  saving: {
    type: Boolean,
    default: false,
  },
})

const categoryOptions = ref(['Eletrônicos', 'Móveis', 'Roupas', 'Utilidades'])
const productObjModel = {
  sku: '',
  name: '',
  category: categoryOptions.value[0],
  price: '',
  stockQuantity: '',
}
const productObj = ref({ ...productObjModel })

watch(
  () => props.obj,
  (newVal) => {
    if (newVal) productObj.value = { ...newVal }
    else productObj.value = { ...productObjModel }
  },
)

const fnClosedByBgCover = () => {
  emit('close')
  productObj.value = { ...productObjModel }
}

const checkProductFields = () => {
  if (!productObj.value.sku) {
    notify.warning($q, 'Por favor, informe o SKU do produto.')
    return
  }
  if (!productObj.value.name) {
    notify.warning($q, 'Por favor, informe o nome do produto.')
    return
  }
  if (!productObj.value.category) {
    notify.warning($q, 'Por favor, informe a categoria do produto.')
    return
  }
  if (!productObj.value.price) {
    notify.warning($q, 'Por favor, informe o preço do produto.')
    return
  }
  if (productObj.value.category === 'Eletrônicos' && productObj.value.price < 50) {
    notify.warning($q, 'Ajuste o preço, o mínimo para eletrônicos é de R$50,00.')
    return
  }
  if (!productObj.value.stockQuantity || productObj.value.stockQuantity < 0) {
    notify.warning($q, 'Por favor, informe uma quantidade positiva do produto no estoque.')
    return
  }
  save()
}

const save = () => {
  emit('save', productObj.value)
}
</script>
<style lang="scss" scoped></style>
