IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[SEO_ContentLoad]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[SEO_ContentLoad]
GO

	
-- =============================================

-- Author:		Vivek Gupta

-- Create date: 15th Oct,2013

-- Description:	Load SEO Content for dealer webiste

-- =============================================

CREATE PROCEDURE [dbo].[SEO_ContentLoad]
	@DealerId INT,
	@DealerTypeId INT,
	@MakeId INT
AS
BEGIN

	SET NOCOUNT ON;

	IF NOT EXISTS(SELECT PageId FROM DealerWebsite_SEOContent WITH(NOLOCK) WHERE DealerId = @DealerId)
	BEGIN
	   IF (@DealerTypeId = 1) --old car dealer
	   BEGIN
			SELECT PageId, PageName, Title , MetaKeywords, MetaDescription, IsActive
			FROM DealerWebsite_SEOContentMaster
			WHERE DealerShipType = @DealerTypeId
			AND   IsActive = 1
			ORDER BY PageId

	   END
	  ELSE IF (@DealerTypeId = 2)--new car dealer
	   BEGIN
			IF EXISTS(SELECT PageId FROM DealerWebsite_SEOContentMaster WITH(NOLOCK) WHERE MakeId = @MakeId)
			BEGIN
				SELECT PageId, PageName, Title , MetaKeywords, MetaDescription, IsActive
				FROM DealerWebsite_SEOContentMaster
				WHERE DealerShipType = @DealerTypeId
				AND MakeId=@MakeId
				AND   IsActive = 1
				ORDER BY PageId
			END
			ELSE
			BEGIN
				SELECT PageId, PageName, Title , MetaKeywords, MetaDescription, IsActive
				FROM DealerWebsite_SEOContentMaster
				WHERE DealerShipType = @DealerTypeId
				AND MakeId IS NULL
				AND   IsActive = 1		
				ORDER BY PageId	
			END
	   END
	  ELSE IF (@DealerTypeId = 3)----old and new car dealer
	   BEGIN
				SELECT PageId, PageName, Title , MetaKeywords, MetaDescription, IsActive
				FROM DealerWebsite_SEOContentMaster
				WHERE DealerShipType IN (1,2)
				AND MakeId IS NULL
				AND   IsActive = 1
				ORDER BY PageId
	   END
	END
	ELSE
	BEGIN
	    SELECT PageId, PageName, Title, MetaKeywords, MetaDescription, IsActive
		FROM DealerWebsite_SEOContent
		WHERE
			 DealerId = @DealerId
		AND  IsActive = 1
		ORDER BY PageId
	END
END

