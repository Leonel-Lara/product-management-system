const success = ($q, msg) => {
  $q.notify({
    icon: 'bi-check',
    message: msg,
  })
}

const warning = ($q, msg) => {
  $q.notify({
    type: 'warning',
    icon: 'bi-exclamation-lg',
    message: msg,
  })
}

export default {
  success,
  warning,
}
