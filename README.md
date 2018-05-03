#Projekteszközök beadandó


Projektünk egy aknakereső játék, amely sikeres build után a következő helyen érhető el:
http://194.182.67.11/MineSweeper/
A projekt Unity-ben készült és egy Asp.net alapú backend található mögötte.

A projekt webes részéhez (WebApplication1) szükséges a Visual Studio ASP.NET modulja. ( ASP.NET projektek IIS Explorert használnak)
A projekt aknakereső része egyszerűen futtatható Unity-ben.

WebApplication1 jelenleg nem tartalmaz grafikus felületet, szerepe csupán annyi, hogy az aknakeresőtől kapott http request-ekre reagáljon. WebApplication1 áll közvetlen kapcsolatban az adatbázissal, ez felel az adatok lekérdezéséért és módosításáért. WebApplication1 csak localhostról fér hozzá az adatbázishoz, ezért 

Az aknakereső apk file-ja androidon telepítés után tesztelhető.

Unity-ben a projekt a következőképpen tesztelhető: Scenes/Menu-t kell megnyitni és azt futtatni Unity-n belül. Offline módban tesztelhető a játék. Unity-n belül az aspect ratio-t 9:16-ra kell állítani, ekkor tesztelhető a játék. Builden belül a felbontás jó, ott nem kell külön állítani rajta.

A build futtatásakor lehet regisztrálni és be is lehet jelentkezni. Bejelentkezés után a Scoreboard-on megtekinthetőek az elért eredmények.

