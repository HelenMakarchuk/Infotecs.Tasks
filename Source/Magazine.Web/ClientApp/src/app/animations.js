"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var animations_1 = require("@angular/animations");
exports.slideInAnimation = animations_1.trigger('routeAnimation', [
    animations_1.transition('articles <=> article', [
        animations_1.style({ position: 'relative' }),
        animations_1.query(':enter, :leave', [
            animations_1.style({
                position: 'absolute',
                top: 0,
                left: 0,
                width: '100%'
            })
        ]),
        animations_1.query(':enter', [
            animations_1.style({ left: '-100%' })
        ]),
        animations_1.query(':leave', animations_1.animateChild()),
        animations_1.group([
            animations_1.query(':leave', [
                animations_1.animate('300ms ease-out', animations_1.style({ left: '100%' }))
            ]),
            animations_1.query(':enter', [
                animations_1.animate('300ms ease-out', animations_1.style({ left: '0%' }))
            ])
        ]),
        animations_1.query(':enter', animations_1.animateChild()),
    ])
]);
