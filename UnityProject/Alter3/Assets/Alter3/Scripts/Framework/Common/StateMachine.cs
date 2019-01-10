using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.common
{
    using System;
    using System.Collections.Generic;

    //http://baba-s.hatenablog.com/entry/2014/02/06/104203

    /// <summary>
    /// ステートマシン
    /// </summary>
    public class StateMachine<T>
    {
        /// <summary>
        /// ステート
        /// </summary>
        private class State
        {
			public T mKey;
            private readonly Action mEnterAct;  // 開始時に呼び出されるデリゲート
            private readonly Action mUpdateAct; // 更新時に呼び出されるデリゲート
            private readonly Action mLateUpdateAct; // LateUpdate時に呼び出されるデリゲート
            private readonly Action mExitAct;   // 終了時に呼び出されるデリゲート

            /// <summary>
            /// コンストラクタ
            /// </summary>
			public State(T key, Action enterAct = null, Action updateAct = null, Action lateUpdateAct = null, Action exitAct = null)
            {
				mKey = key;
				mEnterAct = enterAct ?? delegate { };
                mUpdateAct = updateAct ?? delegate { };
                mLateUpdateAct = lateUpdateAct ?? delegate { };
                mExitAct = exitAct ?? delegate { };
            }

            /// <summary>
            /// 開始します
            /// </summary>
            public void Enter()
            {
                mEnterAct();
            }

            /// <summary>
            /// 更新します
            /// </summary>
            public void Update()
            {
                mUpdateAct();
            }

            public void LateUpdate()
            {
                mLateUpdateAct();
            }

            /// <summary>
            /// 終了します
            /// </summary>
            public void Exit()
            {
                mExitAct();
            }
        }

        private Dictionary<T, State> mStateTable = new Dictionary<T, State>();   // ステートのテーブル
        private State mCurrentState;                              // 現在のステート
        private State mBeforeState;                              // 一つ前のステート

        /// <summary>
        /// ステートを追加します
        /// </summary>
		public void Add(T key, Action enterAct = null, Action updateAct = null, Action lateUpdateAct = null, Action exitAct = null)
        {
			mStateTable.Add(key, new State(key, enterAct, updateAct, lateUpdateAct, exitAct));
        }

        /// <summary>
        /// 現在のステートを設定します
        /// </summary>
        public void SetState(T key)
        {
            mBeforeState = mCurrentState;
            if (mCurrentState != null)
            {
                mCurrentState.Exit();
            }
            mCurrentState = mStateTable[key];
            mCurrentState.Enter();
        }

        /// <summary>
        /// 現在のステートを更新します
        /// Updateのタイミングで呼び出す想定
        /// </summary>
        public void Update()
        {
            if (mCurrentState == null)
            {
                return;
            }
            mCurrentState.Update();
        }

        /// <summary>
        /// 現在のステートを更新します
        /// LateUpdateのタイミングで呼び出す想定
        /// </summary>
        public void LateUpdate()
        {
            if(mCurrentState == null)
            {
                return;
            }
            mCurrentState.LateUpdate();
        }

        /// <summary>
        /// すべてのステートを削除します
        /// </summary>
        public void Clear()
        {
            mStateTable.Clear();
            mCurrentState = null;
        }

		public T GetCurrentState()
		{
			if (mCurrentState == null)
			{
				return default(T);
			}
			return mCurrentState.mKey;
		}

        public T GetBeforeState()
        {
            if (mBeforeState == null)
            {
                return default(T);
            }
            return mBeforeState.mKey;
        }

    }
}
