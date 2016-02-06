// __tests__/firstTest.js
jest.dontMock('../sample');

describe('sum', function() {
 it('adds 1 + 2 to equal 3', function() {
   var sum = require('../sample');
   expect(sum(1, 2)).toBe(3);
 });
});