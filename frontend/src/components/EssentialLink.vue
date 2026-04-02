<template>
  <q-item
    class="rounded-borders animated fadeInLeft q-mt-md"
    style="animation-delay: 450ms"
    active-class="my-menu-link"
    clickable
    :disable="props.disable"
    tag="router"
    :to="props.link"
  >
    <q-item-section v-if="props.icon" avatar>
      <q-icon :name="props.icon" size="sm" />
    </q-item-section>

    <q-item-section>
      <q-item-label class="text-subtitle2">{{ props.title }}</q-item-label>
      <q-item-label class="caption" caption>{{ props.caption }}</q-item-label>
    </q-item-section>
  </q-item>
</template>

<script setup>
const props = defineProps({
  title: {
    type: String,
    required: true,
  },

  caption: {
    type: String,
    default: '',
  },

  link: {
    type: String,
    default: '#',
  },

  icon: {
    type: String,
    default: '',
  },
})
</script>
<style lang="scss" scoped>
.my-menu-link {
  color: white;
  background: $primary;
  .caption {
    color: white;
  }
}
</style>
