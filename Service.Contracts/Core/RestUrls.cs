namespace Service.Contracts.Core
{
    public class RestUrls
    {
        public const string GetTagList                                      = "/TagList/";
        public const string GetAllTags                                      = "/Tags/";
        public const string GetAllObjectsByTagIds                           = "/Tags/tagids/?tagids={listoftagids}";
        public const string GetAllObjectsByTags                             = "/Tags/tags/?tags={listoftags}";
        public const string GetAllTagsByApp                                 = "/Tags/{app}/";
        public const string GetAppObjectsByTagIds                           = "/Tags/{app}/tagids/?tagids={listoftagids}";
        public const string GetAppObjectsByTags                             = "/Tags/{app}/tags/?tags={listoftags}";
        public const string GetAppTagsByObjectIds                           = "/Tags/{app}/objids/?objids={listofobjids}";
        public const string TagObjects                                      = "/Tags/{app}/";
        public const string UnTagObjects                                    = "/Tags/{app}/";

        //public const string GetTags                                       = "/Tags/?name={name}&pageIndex={pageIndex}&pageSize={pageSize}";
        
        //Micoservice to  other service 
        public const string GetVendorRepsForVendor                          = "vendors/{0}/reps/?pageIndex={1}&pageSize={2}";
    }
}
