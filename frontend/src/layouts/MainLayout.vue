<template>
  <q-layout view="lHh Lpr lFf">
    <q-header class="animated fadeInDown" style="animation-delay: 300ms">
      <q-toolbar>
        <q-btn flat dense round icon="menu" aria-label="Menu" @click="toggleLeftDrawer" />

        <q-toolbar-title>MerchWay</q-toolbar-title>

        <q-avatar>
          <img src="src/assets/images/admin.jpg" />
          <q-badge color="green" rounded floating />
        </q-avatar>
      </q-toolbar>
    </q-header>

    <q-drawer v-model="leftDrawerOpen" show-if-above>
      <q-list class="q-px-md">
        <div
          class="row justify-center q-pt-md q-pb-lg animated fadeIn"
          style="animation-delay: 400ms"
        >
          <q-img src="src/assets/images/logo.svg" width="70%" />
        </div>

        <EssentialLink v-for="link in linksList" :key="link.title" v-bind="link" />
      </q-list>
    </q-drawer>

    <q-page-container class="bg-secondary">
      <router-view />
    </q-page-container>
  </q-layout>
</template>

<script setup>
import { ref } from 'vue'
import EssentialLink from 'components/EssentialLink.vue'

const linksList = [
  {
    title: 'Produtos',
    caption: 'Gerencie todos os produtos',
    icon: 'bi-box-seam',
    link: '/produtos',
    disable: false,
  },
  {
    title: 'Categorias',
    caption: 'Gerencie todas as categorias',
    icon: 'bi-tag',
    link: '/categorias',
    disable: true,
  },
  {
    title: 'FAQ',
    caption: 'Gerencie perguntas frequentes',
    icon: 'bi-patch-question',
    link: '/faq',
    disable: true,
  },
]

const leftDrawerOpen = ref(false)

function toggleLeftDrawer() {
  leftDrawerOpen.value = !leftDrawerOpen.value
}
</script>
