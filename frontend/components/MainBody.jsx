import React from 'react';
import Paper from 'material-ui/Paper';
import Card from './Card.jsx';
import $ from 'jquery';
import config from '../config.js';
import CircularProgress from 'material-ui/CircularProgress';
import Center from 'react-center';

const MainBody = React.createClass({
    getInitialState() {
        return {
            loading: true,
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
        this.serverRequest = $.get(config.apiHost+'/users/octocat/gists', function (result) {
            this.setState({
                items: result,
                loading: false
            });
        }.bind(this));
    },

    componentWillUnmount() {
        this.serverRequest.abort();
    },

    render() {
        var paperStyle = {
            width: '90%',
            margin: 'auto',
            marginTop: 10
        }
        
        if (this.state.loading){
            return (
                <Paper style= { paperStyle } zDepth= { 1}>
                    <Center>
                        <CircularProgress size={1}/>
                    </Center>
                </Paper >
            )
        } else {
            return (
                <Paper style= { paperStyle } zDepth= { 1}>
                    { 
                        this.state.items.map ( item => {
                            return (
                                <li key={item.id}> 
                                    {item.owner.login}'s last gist is <a href={item.owner.html_url}> here</a>.
                                </li>
                            )
                        })
                    }
                </Paper >
            )
        }
    }

});

export default MainBody;