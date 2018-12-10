IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_FNGetBusinessType]') 
    AND xtype IN (N'FN', N'IF', N'TF')
)
    DROP FUNCTION [dbo].[TC_FNGetBusinessType]
GO

	-- Created By:    Deepak Tripathi
-- Create date: 14th July 2016
-- Description: Get BusinessType

-- =============================================

CREATE FUNCTION [dbo].[TC_FNGetBusinessType]
    (@InquiryType TINYINT)
    RETURNS TINYINT 

AS     
    BEGIN  
        DECLARE @BusinessType TINYINT     
        SET @BusinessType = CASE WHEN @InquiryType > 2 THEN @InquiryType ELSE 3 END
        RETURN @BusinessType
    END  

