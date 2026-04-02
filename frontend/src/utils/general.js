import { date as dateUtil } from 'quasar'

const dateFormatted = (strDate) => dateUtil.formatDate(strDate, 'DD/MM/YYYY')

const formatMoney = (value, currency = 'BRL') => {
  return new Intl.NumberFormat('pt-BR', {
    style: 'currency',
    currency,
    minimumFractionDigits: 2,
    maximumFractionDigits: 2,
  }).format(value)
}

export default {
  dateFormatted,
  formatMoney,
}
