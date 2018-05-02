#Projekteszközök beadandó

Projektünk egy aknakereső játék, amely sikeres build után a következő helyen érhető el:
http://194.182.67.11/MineSweeper/

A projekt Unity-ben készült és egy Asp.net alapú backend található mögötte.

A projekt webes részéhez (WebApplication1) szükséges a Visual Studio ASP.NET modulja. ( ASP.NET projektek IIS Explorert használnak)
A projekt aknakereső része egyszerűen futtatható Unity-ben.

WebApplication1 jelenleg nem tartalmaz grafikus felületet, szerepe csupán annyi, hogy az aknakeresőtől kapott http request-ekre reagáljon. WebApplication1 áll közvetlen kapcsolatban az adatbázissal, ez felel az adatok lekérdezéséért és módosításáért.

Az aknakereső apk file-ja androidon telepítés után tesztelhető.
