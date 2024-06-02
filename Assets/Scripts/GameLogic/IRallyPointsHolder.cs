using System.Collections.Generic;

public interface IRallyPointsHolder
{
    public void SetRallyPoints(IEnumerable<RallyPoint> rallyPoints);
    public void ClearRallyPoints();
}
