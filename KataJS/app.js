'use strict';
console.log("Execution started");
/////////////////////////////////////////////////

var entry = require('./FibonacciBus/FibonacciBus');
entry.Run();

//////////////////////////////////////////////////
console.log('Press any key to exit');
process.stdin.setRawMode(true);
process.stdin.resume();
process.stdin.on('data', process.exit.bind(process, 0));