jest.dontMock('../Game/CannotPlay.jsx');

import React from 'react';
import ReactDOM from 'react-dom';
import TestUtils from 'react-addons-test-utils';

const CannotPlay = require('../Game/CannotPlay.jsx').default;

describe('CheckboxWithLabel', () => {

  it('changes the text after click', () => {
    var myMock = jest.genMockFunction();
    var cannotPlay = TestUtils.renderIntoDocument(
      <CannotPlay cannotPlayCards={myMock} disabled={false} />
    );

    var cannotPlayNode = ReactDOM.findDOMNode(cannotPlay);

    expect(cannotPlayNode.textContent).toEqual('I can not play a card!');

    TestUtils.Simulate.click(cannotPlayNode);
    expect(myMock).toBeCalled();
  });

});