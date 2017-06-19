/**
 * Created by elang on 2017/6/19.
 */


namespace XcodeBuilder
{
    public class BuildProgress
    {
        private System.Diagnostics.Process m_Progress;

        public BuildProgress(System.Diagnostics.Process process)
        {
            m_Progress = process;
        }

        public void Update()
        {
            if (null != m_Progress)
            {
                if (m_Progress.StandardOutput.EndOfStream)
                {
                    m_Progress = null;
                }
            }
        }
    }
}