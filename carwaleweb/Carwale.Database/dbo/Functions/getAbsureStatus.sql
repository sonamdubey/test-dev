IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[getAbsureStatus]') 
    AND xtype IN (N'FN', N'IF', N'TF')
)
    DROP FUNCTION [dbo].[getAbsureStatus]
GO

	
--SELECT dbo.[getAbsureStatus](1,1,NULL,NULL)

--SELECT [IsSurveyDone],[IsRejected],[FinalWarrantyDate],[Status],AbsureStatus, * FROM AbSure_CarDetails
CREATE function [dbo].[getAbsureStatus](@IsSurveyDone bit, @IsRejected bit, @FinalWarrantyDate DATETIME, @Status TINYINT )
returns tinyint
as
begin
declare @ComputedStatus tinyint

SELECT  @ComputedStatus= (	
					  CASE WHEN   ISNULL(@Status,0) <> 3 AND ISNULL(@IsSurveyDone,0) = 1 AND ISNULL(@IsRejected,0) = 1 AND @FinalWarrantyDate IS NULL		THEN 2 ELSE
					  CASE WHEN ((ISNULL(@Status,0) <> 3 AND ISNULL(@IsRejected,0) = 1 AND @FinalWarrantyDate IS NOT NULL)
						   OR
								 (ISNULL(@Status,0) <> 3 AND ISNULL(@IsSurveyDone,0) = 1 AND ISNULL(@IsRejected,0) = 0)) THEN 1 ELSE
					  CASE WHEN   ISNULL(@Status,0) = 3 THEN 3 END END
					  END
				  ) 
FROM    AbSure_CarDetails CD
return  @ComputedStatus
end
