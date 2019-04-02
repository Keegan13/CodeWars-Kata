
//a*x+b*k=m , x - amount of ppl get off on 2nd station
function calc(k, n, m, x) {
    var prev1 = { a: 1, b: 0 };
    var prev2 = { a: 0, b: 1 };
    var cur = { a: 0, b: 1 } // on board
    var required = { a: 0, b: 0 };
    for (var i = 3; i < n; i++) {
        console.log('iteration ' + i);
        console.log();
        var a = prev2.a + prev1.a;
        var b = prev2.b + prev1.b
        prev2 = prev1;
        prev1 = { a: a, b: b };
        cur.a += a - prev2.a;
        cur.b += b - prev2.b;
        if (i == x) required = { a: cur.a, b: cur.b };
        console.log(prev2, prev1, cur);
    }
    var x = (m - cur.b * k) / cur.a;
    return required.a * x + required.b * k;
}

module.exports.Run = function () {

    console.log(calc(5, 7, 32, 4));
    console.log(calc(12, 23, 212532, 8));
};
