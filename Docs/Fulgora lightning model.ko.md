이 문서는 [영어](Fulgora%20lightning%20model.md)로도 볼 수 있습니다.

이 문서는 `BuildAccumulatorView`에서 `requiredChargeMw`를 계산할 때 사용하는 수식을 설명합니다. 여기서는 이를 "_cap_"이라고 부릅니다.
처음 두 식이 출발점이며, 나머지 식은 _cap_을 구하기 위한 전개입니다.

_chargeTime_은 번개 한 번당 충전 시간입니다.
_eff_는 번개 유인기의 효율입니다.

$$chargeTime=\frac{1000MJ\times eff}{drain+cap+load}$$
$$cap\times chargeTime\times numStrikes-load\times(stormTime-chargeTime\times numStrikes)=reqMj$$
$$\frac{cap\times 1000MJ\times eff\times numStrikes}{drain+cap+load}-load\times\left(stormTime-\frac{1000MJ\times eff\times numStrikes}{drain+cap+load}\right)=reqMj$$
$$\frac{cap\times 1000MJ\times eff\times numStrikes}{drain+cap+load}-load\times stormTime+\left(\frac{load\times 1000MJ\times eff\times numStrikes}{drain+cap+load}\right)=reqMj$$
$$\frac{cap\times 1000MJ\times eff\times numStrikes-load\times stormTime\times(drain+cap+load)+load\times 1000MJ\times eff\times numStrikes}{drain+cap+load}=reqMj$$
$$cap\times 1000MJ\times eff\times numStrikes-load\times stormTime\times(drain+cap+load)+load\times 1000MJ\times eff\times numStrikes\\
=reqMj\times(drain+cap+load)$$
$$cap\times 1000MJ\times eff\times numStrikes-load\times stormTime\times drain-load\times stormTime\times cap\\
-\ load\times stormTime\times load+load\times 1000MJ\times eff\times numStrikes\\
=reqMj\times drain+reqMj\times cap+reqMj\times load$$
$$cap\times 1000MJ\times eff\times numStrikes-load\times stormTime\times cap-reqMj\times cap\\
\begin{aligned}
=\ &reqMj\times drain+reqMj\times load+load\times stormTime\times drain\\
&+load\times stormTime\times load-load\times 1000MJ\times eff\times numStrikes
\end{aligned}$$
$$\begin{aligned}
cap\times(&1000MJ\times eff\times numStrikes-load\times stormTime-reqMj)\\
&=reqMj\times drain+load\times(reqMj+stormTime\times drain+stormTime\times load-1000MJ\times eff\times numStrikes)
\end{aligned}$$
$$cap=\frac{reqMj\times drain+load\times(reqMj+stormTime\times(drain+load)-1000MJ\times eff\times numStrikes)}{1000MJ\times eff\times numStrikes-load\times stormTime-reqMj}$$
