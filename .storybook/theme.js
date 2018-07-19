export const stylesheet = {
  button: {
    base: {
      fontFamily: 'sans-serif',
      fontSize: '12px',
      display: 'block',
      position: 'fixed',
      border: 'none',
      background: '#28c',
      color: '#fff',
      padding: '5px 15px',
      cursor: 'pointer',
      boxShadow: 'none'
    },
    topRight: {
      top: 0,
      right: 0,
      borderRadius: '0 0 0 5px',
    },
  },
  info: {
    position: 'fixed',
    background: 'white',
    top: 0,
    bottom: 0,
    left: 0,
    right: 0,
    padding: '0 40px',
    overflow: 'auto',
    zIndex: 99999,
  },
  children: {
    position: 'relative',
    zIndex: 0,
  },
  infoBody: { // story body container
    fontFamily: '"Lato", sans-serif',
    color: '#324d5b',
    WebkitFontSmoothing: 'antialiased',
    fontWeight: 300,
    lineHeight: 1.45,
    fontSize: '15px',
    padding: '0 20px',
    border: '1px solid transparent',
    borderRadius: '2px',
    boxShadow: 'none',
    backgroundColor: '#fff',
    marginTop: '0px',
    marginBottom: '20px',
  },
  infoContent: {
    marginBottom: 0,
  },
  infoStory: { // story output
    padding: '10px 20px',
  },
  jsxInfoContent: {
    borderTop: '1px solid #f2f1ef',
    margin: '20px 0 0 0',
  },
  header: { // story header
    h1: {
      margin: 0,
      padding: '20px 0 0',
      fontSize: '22px',
    },
    h2: {
      margin: '0 0 20px 0',
      padding: 0,
      fontWeight: 400,
      fontSize: '16px',
      color: '#777',
    },
    body: {
      borderBottom: '1px solid #f2f1ef',
      paddingTop: 0,
      marginBottom: 0,
    },
  },
  source: { // story source and prop types
    h1: {
      margin: '20px 0 0 0',
      padding: '15px 0 10px 0',
      fontSize: '18px',
      borderBottom: '1px solid transparent',
      borderTop: '1px solid #f2f1ef',
    },
  },
  propTableHead: { //prop types' Component head
    fontSize: '14px',
    fontWeight: 'normal',
    margin: '10px 0',
  },
};
