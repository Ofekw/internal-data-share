import React from 'react'
import { Router, Route, NoMatch, IndexRoute } from 'react-router'
import  ParentContainer  from './ParentContainer.jsx';
import  ListNode  from './ListNode.jsx';
import { hashHistory } from 'react-router';

class nodes extends React.Component { 

    getInitalState() {
        // call to get high level routing topology

    }

    render() {
        return (
            <div>
            <h1>routes</h1>
            <Router history={hashHistory}>
                <Route path="/" name="root" component={ParentContainer} > 
                    <Route path="1" name="bank1" component={ListNode} parent="Hello">
                        <Route path="1" name="vm1" component={ListNode}></Route>
                        <Route path="2" name="vm2" component={ListNode}></Route>
                        <Route path="3" name="vm3" component={ListNode}></Route>
                    </Route>
                    <Route path="2" name="bank2" component={ListNode}>
                        <Route path="1" name="vm1" component={ListNode}></Route>
                        <Route path="2" name="vm2" component={ListNode}></Route>
                        <Route path="3" name="vm3" component={ListNode}></Route>
                    </Route>    
                    <Route path="3" name="bank3" component={ListNode}>
                        <Route path="1" name="vm1" component={ListNode}></Route>
                        <Route path="2" name="vm2" component={ListNode}></Route>
                        <Route path="3" name="vm3" component={ListNode}></Route>  
                    </Route>                                       
                </Route>
                <Route name="404: No Match for route" path="*" component={NoMatch}/>
            </Router>
            </div>
        )
    }
};

export default nodes;