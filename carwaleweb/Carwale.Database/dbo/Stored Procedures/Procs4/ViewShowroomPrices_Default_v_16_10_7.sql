IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[ViewShowroomPrices_Default_v_16_10_7]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[ViewShowroomPrices_Default_v_16_10_7]
GO

	-- =============================================
-- Author:		<Anuj Dhar>
-- Create date: <13/10/2016>
-- Description:	<Referenced from SP ViewShowroomPrices_Default>
-- Modified by Anuj Dhar, To add order of category items
-- =============================================
CREATE PROCEDURE [dbo].[ViewShowroomPrices_Default_v_16_10_7]
	@ModelId INT,
	@ItemList Varchar(100)
AS
BEGIN
	SET NOCOUNT ON
	DECLARE @DefaultItems VARCHAR(100) = '2,3,5,77'

	SELECT PQI.Id AS ItemId, PQI.CategoryName, PQI.CategoryId,(
			CASE PQI.Id
				WHEN '77'
					THEN '1'
				ELSE '0'
				END
			) AS OrderColumn
	FROM PQ_CategoryItems PQI WITH (NOLOCK)
	WHERE PQI.Id IN (SELECT Items FROM SplitTextRS(ISNULL(@ItemList,@DefaultItems),','))
	ORDER BY PQI.CategoryId
END

