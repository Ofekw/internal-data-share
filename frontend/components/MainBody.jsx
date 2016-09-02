import React from 'react';
import Paper from 'material-ui/Paper';
import Card from './Card.jsx';
import $ from 'jquery';
import config from '../config.js';
import CircularProgress from 'material-ui/CircularProgress';
import ListParentContainer from './List/ParentContainer.jsx';

var MainBody = React.createClass({
    getInitialState() {
        return {
            loading: false,
            items: []
        };
    },

    enableLoadingMode() {
        this.setState({
            loading: true
        });
    },

    disableLoadingMode() {
        this.setState({
            loading: false
        });
     },

    componentDidMount() {
        this.enableLoadingMode;
    },

    componentWillUnmount() {
        this.serverRequest.abort();
    },

    render() {
        var paperStyle = {
            width: '90%',
            margin: 'auto',
            marginTop: 10
        };
        
        if (this.state.loading){
            return (
                <Paper style= { paperStyle } zDepth= { 1}>
                    <CircularProgress />
                </Paper >
            )
        } else {
            return (
                <Paper style= { paperStyle } zDepth= { 1}>
                    <ListParentContainer editMode={this.props.editMode}/>
                </Paper >
            )
        }
    }
});

export default MainBody;