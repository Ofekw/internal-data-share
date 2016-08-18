import React from 'react';
import {List, ListItem} from 'material-ui/List';
import {Card, CardActions, CardHeader, CardText} from 'material-ui/Card';
import FlatButton from 'material-ui/FlatButton';
import ContentCopy from 'material-ui/svg-icons/content/content-copy';
import EditModeField from './EditModeField.jsx';

const styles = {
  paleGrey: {
    'background-color': '#ddd',
  },
};


class CardExampleExpandable extends React.Component {
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
          <ListItem primaryText="http://ec1-257-0-123-999.ap-northnortheast-2.compute.amazonaws.com:3000" secondaryText="Url" rightIcon={<ContentCopy />} />
          <ListItem primaryText="Secret" secondaryText="Environment Password" rightIcon={<ContentCopy />} />
          <ListItem primaryText="Value 73" secondaryText="Some Other Key" rightIcon={<ContentCopy />} />
          <ListItem><EditModeField/></ListItem>
          <ListItem><EditModeField identifier="Password" value="Secret"/></ListItem>
        </List>
      </Card>
    );
  }

  enterEditMode() {

  }
}
export default CardExampleExpandable;
