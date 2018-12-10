IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[ViewShowroomPrices]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[ViewShowroomPrices]
GO

	-- =============================================
-- Author:		<Raghupathy>
-- Create date: <10/5/2013>
-- Description:	<This Sp is used to get LocalTax Name and Rate, IsTaxonTax field>
-- Modified by Raghu : <10/24/2013> added order by Categoryid 
-- Modified by Raghu : <10/31/2013> checked CV.IsDeleted Flag
-- Modified by Jitendra : <12/10/2015> change sp to grouped metallic and non metallic prices
-- =============================================
/* [dbo].[ViewShowroomPrices] 10,493 */
CREATE PROCEDURE [dbo].[ViewShowroomPrices]
	-- Add the parameters for the stored procedure here
	@CityId		INT,
	@ModelId	INT,
	@OnlyNew	Bit,
	@SolidInd   Bit,
	@MetallicInd Bit
 AS
	BEGIN		
		  
		--SELECT CV.ID AS VersionId, CV.Name VersionName
		--FROM CarVersions CV WITH(NOLOCK)
		--WHERE CV.IsDeleted = 0 AND CV.CarModelId = @ModelId AND New = @OnlyNew
		--ORDER BY New DESC
		IF @SolidInd=1 AND @MetallicInd=1
			BEGIN
					SELECT * FROM 
						(SELECT  
							CV.ID AS VersionId, CV.Name VersionName,Np.PQ_CategoryItemValue, CI.CategoryName ,DATEDIFF(dd, np.LastUpdated, GETDATE()) UpdatedBeforeDays,
							0 as isMetallic,'SOLID' AS Color
						FROM CarVersions CV WITH(NOLOCK)
							left JOIN CW_NewCarShowroomPrices NP  WITH(NOLOCK) ON CV.ID = NP.CarVersionId AND CV.IsDeleted =0 and np.PQ_CategoryItem=2 and NP.CityId = @CityId AND isMetallic=0
							left JOIN PQ_CategoryItems CI WITH(NOLOCK) ON CI.Id = NP.PQ_CategoryItem
							left JOIN PQ_Category PC WITH(NOLOCK) ON PC.CategoryId = CI.CategoryId 
							WHERE CV.IsDeleted = 0 AND New = @OnlyNew  AND CV.CarModelId = @ModelId

						UNION ALL 

						SELECT  
							CV.ID AS VersionId, CV.Name VersionName,Np.PQ_CategoryItemValue, CI.CategoryName ,DATEDIFF(dd, np.LastUpdated, GETDATE()) UpdatedBeforeDays,
							1 as isMetallic,'METALLIC' AS Color
						FROM CarVersions CV WITH(NOLOCK)
							left JOIN CW_NewCarShowroomPrices NP  WITH(NOLOCK) ON CV.ID = NP.CarVersionId AND CV.IsDeleted =0 and np.PQ_CategoryItem=2 and NP.CityId = @CityId AND isMetallic=1
							left JOIN PQ_CategoryItems CI WITH(NOLOCK) ON CI.Id = NP.PQ_CategoryItem
							left JOIN PQ_Category PC WITH(NOLOCK) ON PC.CategoryId = CI.CategoryId 
							WHERE CV.IsDeleted = 0 AND New = @OnlyNew  AND CV.CarModelId = @ModelId) as temp 
					ORDER BY Color DESC ,PQ_CategoryItemValue ASC
					
			END
		ELSE
			BEGIN
				DECLARE @ColorInd BIT
				DECLARE @ColorName VARCHAR(12)

				SET @ColorInd = CASE WHEN @SolidInd = 1 THEN 0 ELSE 1 END
				SET @ColorName = CASE WHEN @SolidInd = 1 THEN 'SOLID' ELSE 'METALLIC' END

				SELECT  CV.ID AS VersionId, CV.Name VersionName,Np.PQ_CategoryItemValue, CI.CategoryName ,DATEDIFF(dd, np.LastUpdated, GETDATE()) UpdatedBeforeDays,@ColorInd as isMetallic,
				@ColorName AS Color 
				FROM CarVersions CV WITH(NOLOCK)
				left JOIN CW_NewCarShowroomPrices NP  WITH(NOLOCK) ON CV.ID = NP.CarVersionId AND CV.IsDeleted =0 and np.PQ_CategoryItem=2 and NP.CityId = @CityId AND isMetallic=@ColorInd
				left JOIN PQ_CategoryItems CI WITH(NOLOCK) ON CI.Id = NP.PQ_CategoryItem
				left JOIN PQ_Category PC WITH(NOLOCK) ON PC.CategoryId = CI.CategoryId 
				WHERE CV.IsDeleted = 0 AND New = @OnlyNew  AND CV.CarModelId = @ModelId
				ORDER BY Np.PQ_CategoryItemValue ASC

			END 

		


		SELECT NP.CarVersionId AS VersionId, CI.CategoryName, Ci.Id as ItemId, Np.PQ_CategoryItemValue AS ItemValue,
				CI.CategoryId,PC.SortOrder,NP.isMetallic
		FROM CW_NewCarShowroomPrices NP WITH(NOLOCK)
		INNER JOIN CarVersions CV WITH(NOLOCK) ON CV.ID = NP.CarVersionId AND CV.CarModelId = @ModelId AND CV.IsDeleted =0
		INNER JOIN PQ_CategoryItems CI WITH(NOLOCK) ON CI.Id = NP.PQ_CategoryItem
		INNER JOIN PQ_Category PC WITH(NOLOCK) ON PC.CategoryId = CI.CategoryId 			
		WHERE NP.CityId = @CityId AND CV.New = @OnlyNew
		--ORDER BY CI.CategoryId --Added by Raghu
		ORDER BY PC.SortOrder
		
END

