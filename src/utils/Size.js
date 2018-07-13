export default function getSize(size) {
  switch (size) {
    case 'large':
      return 'lg';

    case 'small':
      return 'sm';

    default:
      return 'default';
  }
}
