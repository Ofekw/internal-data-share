import React from 'react';
import {List, ListItem} from 'material-ui/List';
import IconMenu from 'material-ui/IconMenu';
import Popover from 'material-ui/Popover';
import Menu from 'material-ui/Menu';
import MenuItem from 'material-ui/MenuItem';
import IconButton from 'material-ui/IconButton/IconButton';
import MoreVertIcon from 'material-ui/svg-icons/navigation/more-vert';
import {Card, CardActions, CardHeader, CardText} from 'material-ui/Card';
import FlatButton from 'material-ui/FlatButton';
import ContentCopy from 'material-ui/svg-icons/content/content-copy';
import NormalModeField from './NormalModeField.jsx';
import EditModeField from './EditModeField.jsx';

const styles = {
  paleGrey: {
    'background-color': '#ddd',
  },
};


class CardExampleExpandable extends React.Component {
  constructor(props) {
    super(props);

    this.state = {
      open: false,
    };
  }

  render() {
    return (
      <Card>
        <CardHeader
          title="ABC Bank VM_1"
          subtitle="Subtitle?"
          actAsExpander={false}
          showExpandableButton={false}
        />
        <CardActions>
          <FlatButton label="Edit" onTouchTap={this.enterEditMode}/>
          <FlatButton label="Delete" />
        </CardActions>
        <List>
          <NormalModeField identifier="Url" value="http://ec1-257-0-123-999.ap-northnortheast-2.compute.amazonaws.com"/>
          <NormalModeField identifier="Password" value="hunter"/>
          <NormalModeField identifier="Some other key" value="Hello World"/>
          <EditModeField/>
          <EditModeField identifier="Password" value="Secret"/>
        </List>
      </Card>
    );
  }

  showMenu() {

  }

  enterEditMode() {

  }
}
export default CardExampleExpandable;
