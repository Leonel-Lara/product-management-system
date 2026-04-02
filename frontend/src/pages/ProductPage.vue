<template>
  <ProductCrud
    :show="showProductCrud"
    :obj="selectedProduct"
    @close="fnCloseProductCrud"
    @save="saveProduct"
  />
  <ProductDetails
    :show="showProductDetails"
    :objId="selectedProductId"
    @close="fnCloseProductDetails"
  />
  <q-page class="flex q-px-md">
    <div class="row items-start full-width full-heigth q-pb-xl">
      <q-table
        flat
        title="Produtos"
        :columns="columns"
        :rows="productList"
        v-model:pagination="pagination"
        @request="onRequest"
        row-key="id"
        class="q-mt-lg col-12 animated zoomIn"
        style="animation-delay: 500ms"
        rows-per-page-label="Resultados por página:"
        no-data-label="Parece que não temos nenhum registro ainda."
        :rows-per-page-options="[0]"
      >
        <template v-slot:body="props">
          <q-tr :props="props">
            <q-td auto-width>
              <q-btn
                @click="fnShowProductDetails(props.row.id)"
                icon="mdi-eye"
                flat
                round
                color="accent"
              />
            </q-td>
            <q-td key="sku" :props="props">{{ props.row.sku }}</q-td>
            <q-td key="name" :props="props">{{ props.row.name }}</q-td>
            <q-td key="category" :props="props">{{ props.row.category }}</q-td>
            <q-td key="price" :props="props">
              {{ generalUtil.formatMoney(props.row.price) }}
            </q-td>
            <q-td key="stockQuantity" :props="props">{{ props.row.stockQuantity }}</q-td>
            <q-td auto-width key="timestamp" :props="props">
              <q-badge>
                {{ generalUtil.dateFormatted(props.row.timestamp) }}
              </q-badge>
            </q-td>
            <q-td auto-width>
              <q-btn
                @click="fnShowProductCrud(props.row)"
                icon="mdi-pencil"
                flat
                round
                color="blue-8"
              />
            </q-td>
            <q-td auto-width>
              <q-btn @click="deleteProduct(props.row)" icon="mdi-delete" flat round color="red-8" />
            </q-td>
          </q-tr>
        </template>
      </q-table>
      <q-inner-loading :showing="loading">
        <q-spinner-grid size="30px" color="primary"></q-spinner-grid>
      </q-inner-loading>
    </div>

    <q-page-sticky
      class="animated fadeInUp"
      style="animation-delay: 300ms"
      position="bottom-right"
      :offset="[18, 18]"
    >
      <q-btn @click="fnShowProductCrud(null)" fab icon="add" color="primary" />
    </q-page-sticky>
  </q-page>
</template>

<script setup>
import { ref } from 'vue'
import { useQuasar } from 'quasar'
import productApi from 'src/api/productApi'
import ProductCrud from 'src/components/ProductCrud.vue'
import ProductDetails from 'src/components/ModalDetails.vue'
import generalUtil from 'src/utils/general'
import notify from 'src/utils/notifications'

const $q = useQuasar()

const columns = [
  {
    name: 'view',
    label: 'Visualizar',
    align: 'left',
  },
  {
    name: 'sku',
    label: 'SKU',
    align: 'left',
  },
  {
    name: 'name',
    label: 'Nome',
    align: 'left',
  },
  {
    name: 'category',
    label: 'Categoria',
    align: 'left',
  },
  {
    name: 'price',
    label: 'Preço',
    align: 'left',
  },
  {
    name: 'stockQuantity',
    label: 'Qntd. Estoque',
    align: 'left',
  },
  {
    name: 'timestamp',
    field: 'timestamp',
    label: 'Data de cadastro',
    align: 'left',
  },
  {
    name: 'edit',
    label: 'Editar',
    align: 'left',
  },
  {
    name: 'delete',
    label: 'Excluir',
    align: 'left',
  },
]

const pagination = ref({
  sortBy: 'desc',
  descending: false,
  page: 1,
  rowsPerPage: 10,
  rowsNumber: 0,
})
const productList = ref([])
const loading = ref(true)
const saving = ref(false)
const showProductCrud = ref(false)
const showProductDetails = ref(false)
const selectedProduct = ref(null)
const selectedProductId = ref(null)

const fetchProducts = async () => {
  const paginationParams = { page: pagination.value.page, pageSize: pagination.value.rowsPerPage }
  try {
    const { data } = await productApi.getList(paginationParams)
    productList.value = data.data
    pagination.value.rowsNumber = data.totalItems
  } finally {
    loading.value = false
  }
}
fetchProducts()

const onRequest = async (props) => {
  const { page, rowsPerPage, sortBy, descending } = props.pagination
  if (loading.value) return

  loading.value = true
  const paginationParams = { page: page, pageSize: rowsPerPage }
  try {
    const { data } = await productApi.getList(paginationParams)
    productList.value = data.data
    productList.value.splice(0, productList.value.length, ...data.data)
    pagination.value.rowsNumber = data.totalItems
    pagination.value.page = page
    pagination.value.rowsPerPage = rowsPerPage
    pagination.value.sortBy = sortBy
    pagination.value.descending = descending
  } finally {
    loading.value = false
  }
}

const fnShowProductDetails = (productId) => {
  selectedProductId.value = productId
  showProductDetails.value = true
}

const fnCloseProductDetails = () => {
  showProductDetails.value = false
  selectedProductId.value = null
}

const fnShowProductCrud = (product) => {
  selectedProduct.value = product
  showProductCrud.value = true
}

const fnCloseProductCrud = () => {
  showProductCrud.value = false
  selectedProduct.value = null
}

const saveProduct = async (product) => {
  saving.value = true
  const requestFunctionName = product.id ? 'putItem' : 'postItem'
  const succesmessage = product.id ? 'Produto atualizado!' : 'Produto criado!'
  try {
    const { status, data } = await productApi[requestFunctionName](product)
    if (status === 200) {
      product.id ? updateProduct(data) : newProductCreated(data)
      notify.success($q, succesmessage)
    }
  } finally {
    saving.value = false
  }
}

const newProductCreated = (product) => {
  fnCloseProductCrud()
  productList.value.unshift(product)
}

const updateProduct = (product) => {
  fnCloseProductCrud()
  if (!product?.id) return
  let productIndex = productList.value.map((p) => p.id).indexOf(product.id)
  if (productIndex > -1) productList.value.splice(productIndex, 1, product)
}

const deleteProduct = (product) => {
  $q.dialog({
    title: 'Excluir produto',
    message: `Tem certeza que deseja excluir o produto?<br/>SKU: <b class="text-weight-bold">${product.sku}</b><br/>Nome: <b class="text-weight-bold">${product.name}</b>`,
    ok: {
      color: 'red-9',
    },
    cancel: {
      color: 'grey-5',
      noCaps: true,
    },
    html: true,
  }).onOk(async () => {
    const { status } = await productApi.deleteItem(product.id)
    if (status === 204) {
      const index = productList.value.indexOf(product)
      if (index > -1) productList.value.splice(index, 1)
      notify.success($q, 'Produto excluído com sucesso!')
    }
  })
}
</script>
