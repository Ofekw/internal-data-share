import React from 'react';
import RaisedButton from 'material-ui/RaisedButton';
import TopBar from './TopBar.jsx';
import MainBody from './MainBody.jsx';
import Paper from 'material-ui/Paper';
import FlatButton from 'material-ui/FlatButton';

var App = React.createClass({

	onGlobalEdit: function(){
		this.setState({editMode: true});
	},

	getInitialState: function(){
		return {
			editMode: false
		}
	},

	render: function(){
		return (
			<div>
      			<TopBar onGlobalEdit={this.onGlobalEdit}/>
     			<MainBody editMode={this.state.editMode}/>
    		</div>
		)
	}
});


export default App;
