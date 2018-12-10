import React from 'react';
import Footer from '../../containers/Footer';
import Header from '../../containers/Header';
import NavigationDrawer from '../../containers/NavigationDrawer';
import AppRoutes from '../routes';

class App extends React.Component {
	render() {
		return (
			<div>
				<Header />
				<AppRoutes />
				<NavigationDrawer />
				<Footer />
			</div>
		)
	}
}

export default App
