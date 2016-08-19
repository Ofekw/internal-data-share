import React from 'react';
import Paper from 'material-ui/Paper';
import Card from './Card.jsx';
import $ from 'jquery';

const MainBody = React.createClass({
    getInitialState() {
        return {
            items: []
        };
    },

    componentDidMount() {
        this.serverRequest = $.get("https://api.github.com/users/octocat/gists", function (result) {
            this.setState({
                items: result
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
        };
        return (
            <Paper style= { paperStyle } zDepth= { 1}>
            <Card/>
            </Paper >
                //            { this.state.items.map(item => {
                //     console.log(item.owner.login)
                //     return <li key={item.id}> {item.owner.login}'s last gist is
                //         <a href={item.owner.html_url}>here</a>.</li>
                // }) }
        )
    }

});

export default MainBody;