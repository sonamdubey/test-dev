IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetTopVersionByPQ]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetTopVersionByPQ]
GO

	-- =============================================
-- Author:		Avishkar Meshram
-- Create date: 21-03-2014
-- Description:	To get top version for a Week based on PQ count
-- =============================================
CREATE PROCEDURE [dbo].[GetTopVersionByPQ]
AS
BEGIN

	TRUNCATE TABLE TopVersionCar;

	with CTE
	as
	(select N.CarVersionId,vw.ModelId,ROW_NUMBER() Over (partition by Modelid order by count(*) desc) as TopVersion,
	count(*) as PQCount
	from NewCarPurchaseInquiries as N with (nolock)
	join vwMMV as vw with (nolock) on vw.VersionId=N.CarVersionId
	where RequestDateTime between getdate()-7 and getdate()
	group by N.CarVersionId,vw.ModelId
	)
	insert into TopVersionCar(Modelid,VersionId,PQCount)
	select Modelid,CarVersionId,PQCount 
	from CTE 
	where TopVersion=1

END 


  