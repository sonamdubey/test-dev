IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[cw].[AutoExpo_MobileDefaultPage]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [cw].[AutoExpo_MobileDefaultPage]
GO

	-- ============================================= 

-- Author:   Supriya Khartode 

-- Create date:  15/1/2014

-- Description:  To fetch videos & images for mobile default page

-- ============================================= 

CREATE PROCEDURE [cw].[AutoExpo_MobileDefaultPage] --Execute cw.AutoExpo_MobileDefaultPage_v2

	-- Add the parameters for the stored procedure here 

AS

BEGIN

	-- SET NOCOUNT ON added to prevent extra result sets from 

	-- interfering with SELECT statements. 

	SET NOCOUNT ON;



	-- Insert statements for procedure here 

	DECLARE @vidCount INT = 0



	SET @vidCount = (

			SELECT Count(*) AS VidCount

			FROM con_editcms_videos CV

			INNER JOIN con_editcms_cars CC ON CC.basicid = CV.basicid

			INNER JOIN con_editcms_basic CB ON CB.id = CV.basicid

			WHERE CB.categoryid = 15

			)



	-- (CC.makeid = @MakeId

	--      OR @MakeId IS NULL) )

	IF (@vidCount > 1)

	BEGIN

		SET @vidCount = 1

	END



	SELECT TOP (1) *

	FROM (

		SELECT ROW_NUMBER() OVER (

				PARTITION BY CC.MakeID ORDER BY CB.LastUpdatedTime DESC

				) AS ROWNUMBER

			,CC.MakeId AS MakeId

			,CV.id

			,videourl

			,CB.Title

		FROM con_editcms_videos CV

		INNER JOIN con_editcms_cars CC ON CC.basicid = CV.basicid

		INNER JOIN con_editcms_basic CB ON CB.id = CV.basicid

			AND CB.categoryid = 15

		) t

	WHERE t.ROWNUMBER = 1



	SELECT TOP (3 - @vidCount) *

	FROM (

		SELECT ROW_NUMBER() OVER (

				PARTITION BY CC.MakeID ORDER BY CI.lastupdatedtime DESC

				) AS ROWNUMBER

			,CC.MakeId

			,CI.id

			,CI.caption

			,CI.imagename

			,CI.hosturl

			,CI.imagepathcustom

			,CI.imagepathlarge

			,CI.imagepathoriginal

			,CI.imagepaththumbnail

		FROM con_editcms_images CI

		INNER JOIN con_editcms_cars cc ON CI.basicid = CC.basicid

		INNER JOIN con_editcms_basic CB ON CB.id = CI.basicid

			AND CB.categoryid = 15

			AND CI.ismainImage=1

		) t

	WHERE t.ROWNUMBER = 1

END
